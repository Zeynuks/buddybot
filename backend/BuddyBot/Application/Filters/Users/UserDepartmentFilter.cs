using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserDepartmentFilter : IFilter<User>
{
    public int? DepartmentId { get; set; }

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( DepartmentId.HasValue )
        {
            query = query.Where( u => u.Team != null && u.Team.DepartmentId == DepartmentId.Value );
        }
        return query;
    }
}