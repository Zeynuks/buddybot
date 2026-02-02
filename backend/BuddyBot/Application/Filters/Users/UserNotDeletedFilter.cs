using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserNotDeletedFilter : IFilter<User>
{
    public IQueryable<User> Apply( IQueryable<User> query ) =>
        query.Where( u => !u.IsDeleted );
}
