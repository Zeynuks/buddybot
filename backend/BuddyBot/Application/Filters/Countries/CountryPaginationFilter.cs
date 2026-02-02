using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Countries;
public class CountryPaginationFilter : IFilter<Country>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<Country> Apply( IQueryable<Country> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
