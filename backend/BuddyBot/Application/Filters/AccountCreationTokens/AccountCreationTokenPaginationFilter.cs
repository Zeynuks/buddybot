using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.AccountCreationTokens;
public class AccountCreationTokenPaginationFilter : IFilter<AccountCreationToken>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<AccountCreationToken> Apply( IQueryable<AccountCreationToken> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
