using Application.CQRSInterfaces;
using Application.Filters.Teams;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Queries.GetTeamsLookup;
public class GetTeamsLookupQueryHandler( 
    ITeamRepository teamRepository,
    ILogger<GetTeamsLookupQuery> logger )
    : QueryBaseHandler<List<Team>, GetTeamsLookupQuery>( logger )
{
    protected override async Task<Result<List<Team>>> HandleImplAsync( GetTeamsLookupQuery query )
    {
        List<IFilter<Team>> filters = new List<IFilter<Team>>
        {
            new TeamDepartmentFilter { DepartmentId = query.DepartmentId },
        };

        List<Team> teams = await teamRepository.GetTeamsLookup( filters );
        return Result<List<Team>>.FromSuccess( teams );
    }
}