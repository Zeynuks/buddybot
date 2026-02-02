using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.AccountCreationTokens;
public class AccountCreationTokenStatusFilter : IFilter<AccountCreationToken>
{
    public AccountCreationTokenStatus? Status { get; set; }

    public IQueryable<AccountCreationToken> Apply( IQueryable<AccountCreationToken> query )
    {
        if ( Status.HasValue )
        {
            query = query.Where( act => act.Status == Status.Value );
        }
        return query;
    }
}
