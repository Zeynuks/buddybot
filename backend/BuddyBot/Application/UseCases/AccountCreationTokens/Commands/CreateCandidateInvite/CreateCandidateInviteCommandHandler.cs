using Application.CQRSInterfaces;
using Application.Interfaces;
using Application.Options;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Results;
using Domain.Enums;

namespace Application.UseCases.AccountCreationTokens.Commands.CreateCandidateInvite;
public class CreateCandidateInviteCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IQrCodeGeneratorService qrCodeGeneratorService,
    IOptions<TelegramBotOptions> telegramBotOptions,
    ILogger<CreateCandidateInviteCommand> logger )
    : CommandBaseHandlerWithResult<CreateCandidateInviteCommand, AccountCreationToken>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( CreateCandidateInviteCommand command )
    {
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

        User candidate;
        if ( command.TeamId > 0 )
        {
            candidate = new User( DateTime.UtcNow, command.TeamId );
        }
        else
        {
            candidate = new User( DateTime.UtcNow );
        }

        UserContactInfo contactInfo = new UserContactInfo(
            candidate.Id,
            command.FirstName ?? string.Empty,
            command.LastName ?? string.Empty
        );
        candidate.SetContactInfo( contactInfo );

        Role? candidateRole = await roleRepository.Get( RoleName.Candidate );
        if ( candidateRole == null )
        {
            return Result<AccountCreationToken>.FromError( "Роль 'Кандидат' не найдена." );
        }
        candidate.Roles.Add( candidateRole );

        if ( command.MentorIds != null && command.MentorIds.Count > 0 )
        {
            List<User> mentors = await userRepository.GetUsersByIds( command.MentorIds );
            candidate.SetMentors( mentors );
        }

        if ( command.HRIds != null && command.HRIds.Count > 0 )
        {
            List<User> hrs = await userRepository.GetUsersByIds( command.HRIds );
            candidate.SetHRs( hrs );
        }

        userRepository.Add( candidate );
        await unitOfWork.CommitAsync();

        token.SetUser( candidate );

        accountCreationTokenRepository.Add( token );
        await unitOfWork.CommitAsync();

        AccountCreationToken? createdToken = await accountCreationTokenRepository.GetByTokenValue( token.TokenValue );

        if ( createdToken == null )
        {
            return Result<AccountCreationToken>.FromError( $"Произошла проблема при сохранении токена." );
        }

        return Result<AccountCreationToken>.FromSuccess( createdToken );
    }
}
