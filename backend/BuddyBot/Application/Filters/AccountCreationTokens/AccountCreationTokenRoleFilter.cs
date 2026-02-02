using Domain.Entities;
using Domain.Enums;

namespace Domain.Filters;

public class AccountCreationTokenRoleFilter : IFilter<AccountCreationToken>
{
    public List<RoleName>? Roles { get; set; } = new();
    public IQueryable<AccountCreationToken> Apply( IQueryable<AccountCreationToken> query )
    {
        if ( Roles != null && Roles.Count > 0 )
        {
            query = query.Where( act => act.User != null && act.User.Roles.Any( r => Roles.Contains( r.RoleName ) ) );
        }
        return query;
    }
}
