using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Commands.RejectOnboardingAccessRequest;
public class RejectOnboardingAccessRequestCommandHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<RejectOnboardingAccessRequestCommand> logger )
    : CommandBaseHandlerWithResult<RejectOnboardingAccessRequestCommand, OnboardingAccessRequest>( logger )
{
    protected override async Task<Result<OnboardingAccessRequest>> HandleImplAsync( RejectOnboardingAccessRequestCommand command )
    {
        OnboardingAccessRequest? onboardingAccessRequest = await onboardingAccessRequestRepository.Get( command.CandidateId );

        if ( onboardingAccessRequest is null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Заявка не найдена." );
        }
        if ( onboardingAccessRequest.RequestOutcome != RequestOutcome.Pending && onboardingAccessRequest.RequestOutcome != RequestOutcome.Scheduled )
        {
            return Result<OnboardingAccessRequest>.FromError( "Невозможно отклонить заявку, которая не находится в статусе 'Pending' или 'Scheduled'." );
        }

        onboardingAccessRequest.RequestOutcome = RequestOutcome.Denied;

        await unitOfWork.CommitAsync();

        return Result<OnboardingAccessRequest>.FromSuccess( onboardingAccessRequest );
    }
}