using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.ActivateUserFromToken;
public class ActivateUserFromTokenCommandHandler(
        IAccountCreationTokenRepository accountCreationTokenRepository,
        ICandidateProcessRepository candidateProcessRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<ActivateUserFromTokenCommand> logger
    ) : CommandBaseHandlerWithResult<ActivateUserFromTokenCommand, User>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( ActivateUserFromTokenCommand command )
    {
        User? existingUser = await userRepository.GetByTelegramId( command.TelegramId );
        if ( existingUser != null )
        {
            return Result<User>.FromError( "Ваш Telegram-аккаунт уже активирован в боте.");
        }

        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue( command.TokenValue );
        if ( token == null )
        {
            return Result<User>.FromError( "Токен не найден." );
        }

        if ( token.ExpireDate.HasValue && token.ExpireDate.Value < DateTime.UtcNow )
        {
            return Result<User>.FromError( "Токен истёк." );
        }

        if ( token.Status != AccountCreationTokenStatus.Issued )
        {
            return Result<User>.FromError( "Токен уже использован или недействителен." );
        }

        User? user = token.User;
        if ( user == null )
        {
            return Result<User>.FromError( "Кандидат не найден." );
        }

        if ( user.ContactInfo == null )
        {
            user.SetContactInfo( new UserContactInfo( user.Id, command.TelegramId ) );
        }
        else
        {
            user.ContactInfo.SetTelegramId( command.TelegramId );
        }

        List<RoleName> roleNames = await userRepository.GetUserRoles( user.Id );

        if ( roleNames.Contains( RoleName.Candidate ) )
        {
            CandidateProcess? process = new CandidateProcess(user.Id, ProcessKind.Preboarding, StepKind.PreboardingStart );
            candidateProcessRepository.Add( process );
        }
        token.Activate();

        await unitOfWork.CommitAsync();

        user = await userRepository.Get( user.Id );

        if ( user == null )
        {
            return Result<User>.FromError( "Кандидат не найден после сохранения." );
        }

        return Result<User>.FromSuccess( user );
    }
}
