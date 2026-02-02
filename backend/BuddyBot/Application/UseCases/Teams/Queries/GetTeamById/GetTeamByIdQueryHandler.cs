using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Queries.GetTeamById;
public class GetTeamByIdQueryHandler (
    ITeamRepository teamRepository,
    ILogger<GetTeamByIdQuery> logger )
    : QueryBaseHandler<Team, GetTeamByIdQuery>( logger )
{
    protected override async Task<Result<Team>> HandleImplAsync( GetTeamByIdQuery query )
    {
        Team? team = await teamRepository.Get( query.Id );
        if ( team == null )
        {
            return Result<Team>.FromError( $"Команда с ID {query.Id} не найдена." );
        }

        return Result<Team>.FromSuccess( team );
    }
}