using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserVerifiedHRFilter : IFilter<User>
{
    public bool OnlyVerifiedHR { get; set; }

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( OnlyVerifiedHR )
        {
            query = query.Where( u =>
                !u.Roles.Any( r => r.RoleName == RoleName.HR ) || ( u.Roles.Any( r => r.RoleName == RoleName.HR )
                    && u.ContactInfo != null && u.ContactInfo.TelegramId > 0 )
            );
        }
        return query;
    }
}
