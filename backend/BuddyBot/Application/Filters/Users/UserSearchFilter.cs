using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Users;
public class UserSearchFilter : IFilter<User>
{
    public string? SearchTerm { get; set; }

    public IQueryable<User> Apply( IQueryable<User> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( u =>
                u.ContactInfo != null && (
                    ( !string.IsNullOrEmpty( u.ContactInfo.FirstName ) && u.ContactInfo.FirstName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( !string.IsNullOrEmpty( u.ContactInfo.LastName ) && u.ContactInfo.LastName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( !string.IsNullOrEmpty( u.ContactInfo.FirstName ) && !string.IsNullOrEmpty( u.ContactInfo.LastName ) &&
                        ( u.ContactInfo.FirstName + " " + u.ContactInfo.LastName ).ToLower().Contains( lowerSearchTerm ) )
                )
            );
        }
        return query;
    }
}
