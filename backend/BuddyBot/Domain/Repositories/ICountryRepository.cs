using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface ICountryRepository : IBaseRepository<Country>
{
    Task<int> CountFilteredCountries( IEnumerable<IFilter<Country>> filters );
    Task<List<Country>> GetFilteredCountries( IEnumerable<IFilter<Country>> filters );
    Task<List<Country>> GetCountriesLookup();
}
