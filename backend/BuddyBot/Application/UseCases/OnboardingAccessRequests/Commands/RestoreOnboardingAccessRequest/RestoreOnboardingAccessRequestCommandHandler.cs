using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Commands.RestoreOnboardingAccessRequest;
public class RestoreOnboardingAccessRequestCommandHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<RestoreOnboardingAccessRequestCommand> logger )
    : CommandBaseHandlerWithResult<RestoreOnboardingAccessRequestCommand, OnboardingAccessRequest>( logger )
{
    protected override async Task<Result<OnboardingAccessRequest>> HandleImplAsync( RestoreOnboardingAccessRequestCommand command )
    {
        OnboardingAccessRequest? request = await onboardingAccessRequestRepository.Get( command.CandidateId );

        if ( request is null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Заявка не найдена." );
        }

        if ( request.RequestOutcome != RequestOutcome.Denied )
        {
            return Result<OnboardingAccessRequest>.FromError( "Восстановление доступно только для отозванных заявок." );
        }

        request.RequestOutcome = RequestOutcome.Pending;

        await unitOfWork.CommitAsync();

        return Result<OnboardingAccessRequest>.FromSuccess( request );
    }
}
