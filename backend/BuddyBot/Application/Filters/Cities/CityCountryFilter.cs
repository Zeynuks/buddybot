using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Cities;
public class CityCountryFilter : IFilter<City>
{
    public int? CountryId { get; set; }

    public IQueryable<City> Apply( IQueryable<City> query )
    {
        if ( CountryId.HasValue )
        {
            query = query.Where( c => c.CountryId == CountryId.Value );
        }
        return query;
    }
}
