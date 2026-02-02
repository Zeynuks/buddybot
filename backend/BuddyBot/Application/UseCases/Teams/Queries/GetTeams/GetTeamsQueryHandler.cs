using Application.CQRSInterfaces;
using Application.Filters.Teams;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Teams.Queries.GetTeams;
public class GetTeamsQueryHandler (
    ITeamRepository teamRepository,
    ILogger<GetTeamsQuery> logger )
    : QueryBaseHandler<PagedResult<Team>, GetTeamsQuery>(logger)
{
    protected override async Task<Result<PagedResult<Team>>> HandleImplAsync( GetTeamsQuery query )
    {
        List<IFilter<Team>> filters = new List<IFilter<Team>>
        {
            new TeamSearchFilter { SearchTerm = query.SearchTerm },
            new TeamDepartmentFilter { DepartmentId = query.DepartmentId },
        };

        int totalCount = await teamRepository.CountFilteredTeams( filters );
        filters.Add( new TeamPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<Team> teams = await teamRepository.GetFilteredTeams( filters );

        return Result<PagedResult<Team>>.FromSuccess( new PagedResult<Team>
        {
            Items = teams,
            TotalCount = totalCount
        } );
    }
}
