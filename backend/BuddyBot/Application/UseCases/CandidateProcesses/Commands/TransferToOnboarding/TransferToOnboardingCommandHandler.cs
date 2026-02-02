using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Commands.TransferToOnboarding;
public class TransferToOnboardingCommandHandler(
    ICandidateProcessRepository candidateProcessRepository,
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<TransferToOnboardingCommand> logger
) : CommandBaseHandlerWithResult<TransferToOnboardingCommand, CandidateProcess>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( TransferToOnboardingCommand command )
    {
        OnboardingAccessRequest? onboardingRequest = await onboardingAccessRequestRepository.Get( command.CandidateId );
        if ( onboardingRequest != null && onboardingRequest.RequestOutcome == RequestOutcome.Scheduled )
        {
            onboardingRequest.RequestOutcome = RequestOutcome.Granted;
        }

        CandidateProcess? preboarding = await candidateProcessRepository.Get( command.CandidateId, ProcessKind.Preboarding );
        if ( preboarding != null && preboarding.IsActive )
        {
            preboarding.SetActive( false );
        }

        CandidateProcess? onboarding = await candidateProcessRepository.Get( command.CandidateId, ProcessKind.Onboarding );
        if ( onboarding != null )
        {
            onboarding.SetCurrentStep( StepKind.OnboardingPendingStart );
            onboarding.SetActive( true );
        }
        else
        {
            onboarding = new CandidateProcess( command.CandidateId, ProcessKind.Onboarding, StepKind.OnboardingPendingStart );
            candidateProcessRepository.Add( onboarding );
        }

        await unitOfWork.CommitAsync();

        return Result<CandidateProcess>.FromSuccess( onboarding );
    }
}
