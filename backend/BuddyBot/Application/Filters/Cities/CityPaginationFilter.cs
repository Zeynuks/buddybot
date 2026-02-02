using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Cities;
public class CityPaginationFilter : IFilter<City>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<City> Apply( IQueryable<City> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
