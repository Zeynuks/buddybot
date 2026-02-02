using Application.Filters;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<int> CountFilteredCities( IEnumerable<IFilter<City>> filters )
    {
        IQueryable<City> query = _dbContext.Set<City>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public async Task<List<City>> GetCitiesLookup( IEnumerable<IFilter<City>> filters )
    {
        IQueryable<City> query = _dbContext.Set<City>()
            .OrderBy( c => c.Name );

        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }

    public async Task<List<City>> GetFilteredCities( IEnumerable<IFilter<City>> filters )
    {
        IQueryable<City> query = _dbContext.Set<City>()
            .Include( c => c.Country )
            .Include( c => c.Candidates ).ThenInclude( uc => uc.User ).ThenInclude( u => u.Roles );

        query = query.OrderByDescending( c => c.Id );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }
}
