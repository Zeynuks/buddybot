using Application.Filters;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    public CountryRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<int> CountFilteredCountries( IEnumerable<IFilter<Country>> filters )
    {
        IQueryable<Country> query = _dbContext.Set<Country>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public override async Task<Country?> Get( int id )
    {
        return await _dbContext.Set<Country>()
            .Include( c => c.Cities )
            .FirstOrDefaultAsync( c => c.Id == id );
    }

    public async Task<List<Country>> GetCountriesLookup()
    {
        return await _dbContext.Set<Country>()
            .OrderBy( c => c.Name )
            .ToListAsync();
    }

    public async Task<List<Country>> GetFilteredCountries( IEnumerable<IFilter<Country>> filters )
    {
        IQueryable<Country> query = _dbContext.Set<Country>()
            .Include( c => c.Cities );

        query = query.OrderByDescending( c => c.Id );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }
}
