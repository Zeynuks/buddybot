using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Commands.ResetCandidateProcess;
public class ResetCandidateProcessCommandHandler(
    ICandidateProcessRepository candidateProcessRepository,
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<ResetCandidateProcessCommand> logger
) : CommandBaseHandlerWithResult<ResetCandidateProcessCommand, CandidateProcess>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( ResetCandidateProcessCommand command )
    {
        OnboardingAccessRequest? onboardingRequest = await onboardingAccessRequestRepository.Get( command.CandidateId );
        if ( onboardingRequest != null )
        {
            await onboardingAccessRequestRepository.Delete( command.CandidateId );
        }

        User? candidate = await userRepository.Get( command.CandidateId );
        if ( candidate != null )
        {
            candidate.ResetOnboardingAccessTime();
        }

        List<CandidateProcess> processes = await candidateProcessRepository.GetAll( command.CandidateId );

        CandidateProcess? preboarding = processes.FirstOrDefault( p => p.ProcessKind == ProcessKind.Preboarding );

        if ( preboarding == null )
        {
            return Result<CandidateProcess>.FromError( "Процесс Preboarding не найден для кандидата." );
        }

        foreach ( CandidateProcess process in processes )
        {
            switch ( process.ProcessKind )
            {
                case ProcessKind.Preboarding:
                    process.SetCurrentStep( StepKind.PreboardingStart );
                    process.SetActive( true );
                    break;
                case ProcessKind.Onboarding:
                    process.SetCurrentStep( StepKind.OnboardingPendingStart );
                    process.SetActive( false );
                    break;
                case ProcessKind.PersonalArea:
                    process.SetCurrentStep( StepKind.PersonalAreaHome );
                    process.SetActive( false );
                    break;
            }
        }

        await unitOfWork.CommitAsync();

        return Result<CandidateProcess>.FromSuccess( preboarding );
    }
}