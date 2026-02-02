using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Commands.UpdateTeam;
public class UpdateTeamCommandHandler(
    ITeamRepository repository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateTeamCommand> logger
) : CommandBaseHandlerWithResult<UpdateTeamCommand, Team>( logger )
{
    protected override async Task<Result<Team>> HandleImplAsync( UpdateTeamCommand command )
    {
        Team? team = await repository.Get( command.Id );
        if ( team == null )
        {
            return Result<Team>.FromError( $"Команда с ID {command.Id} не найдена." );
        }

        if ( !string.IsNullOrWhiteSpace( command.Name ) )
        {
            team.SetName( command.Name );
        }

        if ( command.DepartmentId != team.DepartmentId )
        {
            team.SetDepartment( command.DepartmentId );
        }

        if ( command.LeaderId.HasValue )
        {
            if ( team.LeaderId != command.LeaderId )
            {
                var leader = await userRepository.Get( command.LeaderId.Value );
                if ( leader == null )
                    return Result<Team>.FromError( $"Руководитель с ID {command.LeaderId} не найден." );

                team.AssignLeader( leader );
            }
        }
        else
        {
            team.RemoveLeader();
        }

        await unitOfWork.CommitAsync();

        return Result<Team>.FromSuccess( team );
    }
}