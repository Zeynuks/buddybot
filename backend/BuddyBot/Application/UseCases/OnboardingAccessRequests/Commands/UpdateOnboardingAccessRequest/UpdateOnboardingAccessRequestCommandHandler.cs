using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Commands.UpdateOnboardingAccessRequest;
public class UpdateOnboardingAccessRequestCommandHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    IUserRepository userRepository,
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateOnboardingAccessRequestCommand> logger )
    : CommandBaseHandlerWithResult<UpdateOnboardingAccessRequestCommand, OnboardingAccessRequest>( logger )
{
    protected override async Task<Result<OnboardingAccessRequest>> HandleImplAsync( UpdateOnboardingAccessRequestCommand command )
    {
        OnboardingAccessRequest? onboardingAccessRequest = await onboardingAccessRequestRepository.Get( command.CandidateId );

        if ( onboardingAccessRequest is null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Заявка не найдена." );
        }
        if ( onboardingAccessRequest.RequestOutcome != RequestOutcome.Scheduled )
        {
            return Result<OnboardingAccessRequest>.FromError( "Невозможно редактировать заявку, которая не находится в статусе 'Scheduled'." );
        }
        if ( onboardingAccessRequest.Candidate == null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Отсутствует кандидат в заявке." );
        }

        Team? team = null;
        if ( command.TeamId.HasValue )
        {
            team = await teamRepository.Get( command.TeamId.Value );
            if ( team == null )
            {
                return Result<OnboardingAccessRequest>.FromError( "Указанная команда не найдена." );
            }
        }

        onboardingAccessRequest.Candidate.SetTeam( team );
        onboardingAccessRequest.Candidate.SetOnboardingAccessTime( command.OnboardingAccessTimeUtc );

        if ( command.MentorIds != null && command.MentorIds.Count > 0 )
        {
            List<User> mentors = await userRepository.GetUsersByIds( command.MentorIds );
            onboardingAccessRequest.Candidate.SetMentors( mentors );
        }

        await unitOfWork.CommitAsync();

        onboardingAccessRequest = await onboardingAccessRequestRepository.Get( command.CandidateId );

        if ( onboardingAccessRequest is null )
        {
            return Result<OnboardingAccessRequest>.FromError( "Заявка не найдена после сохранения." );
        }

        return Result<OnboardingAccessRequest>.FromSuccess( onboardingAccessRequest );
    }
}

