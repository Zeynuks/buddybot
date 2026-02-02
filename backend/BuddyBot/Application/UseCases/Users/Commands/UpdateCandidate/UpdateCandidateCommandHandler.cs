using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Commands.UpdateCandidate;
public class UpdateCandidateCommandHandler(
    IUserRepository userRepository,
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCandidateCommand> logger )
    : CommandBaseHandlerWithResult<UpdateCandidateCommand, User>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( UpdateCandidateCommand command )
    {
        User? candidate = await userRepository.Get( command.Id );
        if ( candidate == null )
        {
            return Result<User>.FromError( $"Кандидат с ID {command.Id} не найден." );
        }

        if ( candidate.ContactInfo != null )
        {
            if ( !string.IsNullOrWhiteSpace( command.FirstName ) )
            {
                candidate.ContactInfo.SetFirstName( command.FirstName );
            }

            if ( !string.IsNullOrWhiteSpace( command.LastName ) )
            {
                candidate.ContactInfo.SetLastName( command.LastName );
            }
        }

        if ( command.TeamId.HasValue )
        {
            Team? team = await teamRepository.Get( command.TeamId.Value );
            if ( team == null )
            {
                return Result<User>.FromError( "Команда не найдена." );
            }
            candidate.SetTeam( team );
        }

        if ( command.HRIds != null && command.HRIds.Any() )
        {
            List<User> hrs = await userRepository.GetUsersByIds( command.HRIds );
            candidate.SetHRs( hrs );
        }

        if ( command.MentorIds != null && command.MentorIds.Any() )
        {
            List<User> mentors = await userRepository.GetUsersByIds( command.MentorIds );
            candidate.SetMentors( mentors );
        }

        //if ( candidate.CandidateProcesses.Any() && !string.IsNullOrWhiteSpace( command.CurrentStep ) )
        //{
        //    if ( Enum.TryParse<StepKind>( command.CurrentStep, out var stepKind ) )
        //    {
        //        candidate.CandidateProcesses.First().CurrentStep = stepKind;
        //    }
        //    else
        //    {
        //        return Result<User>.FromError( $"Этап '{command.CurrentStep}' не найден." );
        //    }
        //}

        await unitOfWork.CommitAsync();
        return Result<User>.FromSuccess( candidate );
    }
}
