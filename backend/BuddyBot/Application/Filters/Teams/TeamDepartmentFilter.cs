using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Teams;
public class TeamDepartmentFilter : IFilter<Team>
{
    public int? DepartmentId { get; set; }

    public IQueryable<Team> Apply( IQueryable<Team> query )
    {
        if ( DepartmentId.HasValue )
        {
            query = query.Where( t => t.DepartmentId == DepartmentId.Value );
        }
        return query;
    }
}
