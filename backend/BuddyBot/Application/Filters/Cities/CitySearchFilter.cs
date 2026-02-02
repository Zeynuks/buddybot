using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Cities;
public class CitySearchFilter : IFilter<City>
{
    public string? SearchTerm { get; set; }

    public IQueryable<City> Apply( IQueryable<City> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( c => c.Name.ToLower().Contains( lowerSearchTerm ) );
        }
        return query;
    }
}
