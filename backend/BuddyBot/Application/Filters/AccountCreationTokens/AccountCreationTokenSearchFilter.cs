using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.AccountCreationTokens;
public class AccountCreationTokenSearchFilter : IFilter<AccountCreationToken>
{
    public string? SearchTerm { get; set; }

    public IQueryable<AccountCreationToken> Apply( IQueryable<AccountCreationToken> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( act =>
                act.User != null &&
                act.User.ContactInfo != null &&
                (
                    ( act.User.ContactInfo.FirstName != null && act.User.ContactInfo.FirstName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( act.User.ContactInfo.LastName != null && act.User.ContactInfo.LastName.ToLower().Contains( lowerSearchTerm ) )
                )
            );
        }
        return query;
    }
}
