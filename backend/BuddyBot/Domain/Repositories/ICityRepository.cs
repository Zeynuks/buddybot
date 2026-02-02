using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface ICityRepository : IBaseRepository<City>
{
    Task<int> CountFilteredCities( IEnumerable<IFilter<City>> filters );
    Task<List<City>> GetFilteredCities( IEnumerable<IFilter<City>> filters );
    Task<List<City>> GetCitiesLookup( IEnumerable<IFilter<City>> filters );
}