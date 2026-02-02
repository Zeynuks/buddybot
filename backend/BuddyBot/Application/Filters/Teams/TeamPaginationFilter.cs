using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Teams;
public class TeamPaginationFilter : IFilter<Team>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<Team> Apply( IQueryable<Team> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
