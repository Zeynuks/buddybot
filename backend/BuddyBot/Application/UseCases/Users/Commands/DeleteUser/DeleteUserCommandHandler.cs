using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.DeleteUser;
internal class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IAccountCreationTokenRepository accountCreationTokenRepository,
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUserCommand> logger )
    : CommandBaseHandlerWithResult<DeleteUserCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteUserCommand command )
    {
        User? user = await userRepository.Get( command.Id );
        if ( user == null )
        {
            return Result<string>.FromError( $"Пользователь с ID {command.Id} не найден." );
        }

        AccountCreationToken? token = await accountCreationTokenRepository.GetByUserId( user.Id );
        if ( token is not null )
        {
            token.SoftDelete();
        }

        OnboardingAccessRequest? onboardingRequest = await onboardingAccessRequestRepository.Get( user.Id );
        if ( onboardingRequest is not null )
        {
            onboardingRequest.SoftDelete();
        }

        user.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Пользователь с ID {command.Id} был успешно удалён." );
    }
}
