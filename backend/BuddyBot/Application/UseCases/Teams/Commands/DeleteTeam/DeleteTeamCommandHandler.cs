using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Commands.DeleteTeam;
public class DeleteTeamCommandHandler(
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTeamCommand> logger )
    : CommandBaseHandlerWithResult<DeleteTeamCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteTeamCommand command )
    {
        Team? team = await teamRepository.Get( command.Id );
        if ( team == null )
        {
            return Result<string>.FromError( $"Команда с ID {command.Id} не найдена." );
        }

        if ( team.Members?.Any( u => !u.IsDeleted ) == true )
        {
            return Result<string>.FromError( $"Нельзя удалить команду, пока в ней есть участники." );
        }

        team.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Команда с ID {command.Id} была успешно удалена." );
    }
}