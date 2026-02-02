using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserRoleFilter : IFilter<User>
{
    public List<RoleName>? Roles { get; set; } = new();
    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( Roles != null && Roles.Count > 0 )
        {
            query = query.Where( u => u.Roles.Any( r => Roles.Contains( r.RoleName ) ) );
        }
        return query;
    }
}

