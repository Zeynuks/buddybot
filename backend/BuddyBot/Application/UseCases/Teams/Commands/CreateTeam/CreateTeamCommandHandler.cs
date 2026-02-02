using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Commands.CreateTeam;
public class CreateTeamCommandHandler(
    ITeamRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateTeamCommand> logger )
    : CommandBaseHandlerWithResult<CreateTeamCommand, Team>( logger )
{
    protected override async Task<Result<Team>> HandleImplAsync( CreateTeamCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Name ) )
        {
            return Result<Team>.FromError( "Имя команды не может быть пустым." );
        }
        if ( command.DepartmentId <= 0 )
        {
            return Result<Team>.FromError( "Отдел должен быть указан корректно." );
        }

        Team? team = new Team( command.DepartmentId, command.Name );

        repository.Add( team );
        await unitOfWork.CommitAsync();

        return Result<Team>.FromSuccess( team );
    }
}
