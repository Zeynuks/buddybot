using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Commands.UpdateToken;
public class UpdateTokenCommandHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IUnitOfWork unitOfWork,
    ITeamRepository teamRepository,
    IUserRepository userRepository,
    ILogger<UpdateTokenCommand> logger )
    : CommandBaseHandlerWithResult<UpdateTokenCommand, AccountCreationToken>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( UpdateTokenCommand command )
    {
        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue( command.TokenValue );
        if ( token == null )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение с токеном {command.TokenValue} не найдено." );
        }

        if ( token.Status != AccountCreationTokenStatus.Issued )
        {
            return Result<AccountCreationToken>.FromError( "Редактировать можно только неактивированный токен" );
        }

        if ( command.ExpireDate.HasValue )
        {
            token.SetExpireDate( command.ExpireDate.Value );
        }

        User? user = token.User;
        if ( user == null )
        {
            return Result<AccountCreationToken>.FromError( "Пользователь не найден." );
        }

        if ( !string.IsNullOrWhiteSpace( command.FirstName ) || !string.IsNullOrWhiteSpace( command.LastName ) )
        {
            UserContactInfo contactInfo = user.ContactInfo ?? new UserContactInfo( user.Id, command.FirstName ?? string.Empty, command.LastName ?? string.Empty );

            if ( !string.IsNullOrWhiteSpace( command.FirstName ) )
            {
                contactInfo.SetFirstName( command.FirstName );
            }
            if ( !string.IsNullOrWhiteSpace( command.LastName ) )
            {
                contactInfo.SetLastName( command.LastName );
            }
            user.SetContactInfo( contactInfo );
        }

        if ( command.TeamId.HasValue )
        {
            Team? team = await teamRepository.Get( command.TeamId.Value );
            if ( team == null )
            {
                return Result<AccountCreationToken>.FromError( "Команда не найдена." );
            }
            user.SetTeam( team );
        }

        if ( user.IsCandidate() )
        {
            if ( command.MentorIds != null )
            {
                List<User> mentors = await userRepository.GetUsersByIds( command.MentorIds );
                user.SetMentors( mentors );
            }
            if ( command.HRIds != null )
            {
                List<User> hrs = await userRepository.GetUsersByIds( command.HRIds );
                user.SetHRs( hrs );
            }
        }

        if ( user.IsHR() )
        {
            if ( !string.IsNullOrWhiteSpace( command.TelegramContact ) )
            {
                UserContactInfo contactInfo = user.ContactInfo ?? new UserContactInfo( user.Id, string.Empty, string.Empty );
                contactInfo.SetTelegramContact( command.TelegramContact );
                user.SetContactInfo( contactInfo );
            }
        }

        await unitOfWork.CommitAsync();

        return Result<AccountCreationToken>.FromSuccess( token );
    }
}
