using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Countries;
public class CountrySearchFilter : IFilter<Country>
{
    public string? SearchTerm { get; set; }

    public IQueryable<Country> Apply( IQueryable<Country> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( c => c.Name.ToLower().Contains( lowerSearchTerm ) );
        }
        return query;
    }
}
