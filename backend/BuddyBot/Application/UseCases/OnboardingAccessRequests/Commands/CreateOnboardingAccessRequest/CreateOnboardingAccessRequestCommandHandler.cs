using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Commands.CreateOnboardingAccessRequest;
public class CreateOnboardingAccessRequestCommandHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateOnboardingAccessRequestCommand> logger )
    : CommandBaseHandlerWithResult<CreateOnboardingAccessRequestCommand, OnboardingAccessRequest>( logger )
{
    protected override async Task<Result<OnboardingAccessRequest>> HandleImplAsync( CreateOnboardingAccessRequestCommand command )
    {
        OnboardingAccessRequest? existing = await onboardingAccessRequestRepository.Get( command.CandidateId );
        if ( existing != null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Заявка уже создана для этого кандидата." );
        }

        User? candidate = await userRepository.Get( command.CandidateId );
        if ( candidate == null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Кандидат не найден." );
        }

        OnboardingAccessRequest onboardingAccessRequest;

        if ( command.Outcome is null )
        {
            onboardingAccessRequest = new OnboardingAccessRequest( command.CandidateId );
            onboardingAccessRequestRepository.Add( onboardingAccessRequest );
        }
        else if ( command.Outcome == RequestOutcome.Scheduled )
        {
            onboardingAccessRequest = new OnboardingAccessRequest( command.CandidateId, RequestOutcome.Scheduled );
            onboardingAccessRequestRepository.Add( onboardingAccessRequest );
            candidate.SetOnboardingAccessTime( DateTime.UtcNow );
            candidate.SetOnboardingAccessRequest( onboardingAccessRequest );
        }
        else
        {
            return Result<OnboardingAccessRequest>.FromSuccess( null! );
        }

        await unitOfWork.CommitAsync();

        return Result<OnboardingAccessRequest>.FromSuccess( onboardingAccessRequest );
    }
}