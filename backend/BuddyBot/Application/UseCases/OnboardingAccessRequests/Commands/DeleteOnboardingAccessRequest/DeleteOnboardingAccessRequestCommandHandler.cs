using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Commands.DeleteOnboardingAccessRequest;
internal class DeleteOnboardingAccessRequestCommandHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteOnboardingAccessRequestCommand> logger )
    : CommandBaseHandlerWithResult<DeleteOnboardingAccessRequestCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteOnboardingAccessRequestCommand command )
    {
        OnboardingAccessRequest? request = await onboardingAccessRequestRepository.Get( command.Id );
        if ( request == null )
        {
            return Result<string>.FromError( $"Заявка с ID {command.Id} не найдена." );
        }

        request.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Заявка с ID {command.Id} была успешно удалена." );
    }
}