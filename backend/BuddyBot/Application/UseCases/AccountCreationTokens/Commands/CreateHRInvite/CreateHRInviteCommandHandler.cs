using Application.CQRSInterfaces;
using Application.Interfaces;
using Application.Options;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Results;
using Application.PasswordHasher;
using Domain.Enums;

namespace Application.UseCases.AccountCreationTokens.Commands.CreateHRInvite;
public class CreateHRInviteCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork,
    IQrCodeGeneratorService qrCodeGeneratorService,
    IPasswordHasher passwordHasher,
    IOptions<TelegramBotOptions> telegramBotOptions,
    ILogger<CreateHRInviteCommand> logger )
    : CommandBaseHandlerWithResult<CreateHRInviteCommand, AccountCreationToken>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( CreateHRInviteCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Login ) || string.IsNullOrWhiteSpace( command.Password ) )
        {
            return Result<AccountCreationToken>.FromError( "Логин и пароль обязательны для HR." );
        }

        User? existing = await userRepository.GetByLogin( command.Login );
        if ( existing != null )
        {
            return Result<AccountCreationToken>.FromError( "Пользователь с таким логином уже существует." );
        }

        DateTime expireDate = DateTime.UtcNow.AddDays( command.ExpirationDays );
        AccountCreationToken token = new AccountCreationToken( expireDate );

        string? telegramBaseUrl = telegramBotOptions.Value.BaseUrl;
        string telegramInviteLink = $"{telegramBaseUrl}{token.TokenValue}";
        string qrCodeBase64 = qrCodeGeneratorService.GenerateQrCodeBase64( telegramInviteLink );
        token.SetInviteData( telegramInviteLink, qrCodeBase64 );

        User? creator = await userRepository.Get( command.CreatorId );
        if ( creator == null )
        {
            return Result<AccountCreationToken>.FromError( $"Пользователь с Id {command.CreatorId} не найден." );
        }
        token.SetCreator( creator );

        string hashedPassword = passwordHasher.GeneratePassword( command.Password );

        User hr = new User( DateTime.UtcNow, command.Login, hashedPassword );
        hr.SetHRInfo( new HRInfo( hr.Id ) );

        UserContactInfo contactInfo = new UserContactInfo( 
            hr.Id, 
            command.FirstName ?? string.Empty, 
            command.LastName ?? string.Empty
        );
        if ( !string.IsNullOrWhiteSpace( command.TelegramContact ) )
        {
            contactInfo.SetTelegramContact( command.TelegramContact );
        }
        if ( !string.IsNullOrWhiteSpace( command.MicrosoftTeamsUrl ) )
        {
            contactInfo.AddMicrosoftTeamsLink( command.MicrosoftTeamsUrl );
        }
        hr.SetContactInfo( contactInfo );

        Role? hrRole = await roleRepository.Get( RoleName.HR );
        if ( hrRole == null )
        {
            return Result<AccountCreationToken>.FromError( "Роль 'HR' не найдена." );
        }
        hr.Roles.Add( hrRole );

        Team? team = await teamRepository.Get( command.TeamId );
        if ( team == null )
        {
            return Result<AccountCreationToken>.FromError( "Команда не найдена." );
        }
        hr.SetTeam( team );

        userRepository.Add( hr );
        await unitOfWork.CommitAsync();

        token.SetUser( hr );

        accountCreationTokenRepository.Add( token );
        await unitOfWork.CommitAsync();

        AccountCreationToken? createdToken = await accountCreationTokenRepository.GetByTokenValue( token.TokenValue );

        if ( createdToken == null )
        {
            return Result<AccountCreationToken>.FromError( "Произошла проблема при сохранении токена." );
        }

        return Result<AccountCreationToken>.FromSuccess( createdToken );
    }
}
