using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserTeamFilter : IFilter<User>
{
    public int? TeamId { get; set; }

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( TeamId.HasValue )
        {
            query = query.Where( u => u.TeamId == TeamId.Value );
        }
        return query;
    }
}