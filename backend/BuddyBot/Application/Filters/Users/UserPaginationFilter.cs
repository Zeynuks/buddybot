using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserPaginationFilter : IFilter<User>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
