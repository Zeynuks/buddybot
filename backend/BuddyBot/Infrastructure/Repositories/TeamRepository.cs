using Application.Filters;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class TeamRepository : BaseRepository<Team>, ITeamRepository
{
    public TeamRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    { 
    }

    public async Task<int> CountFilteredTeams( IEnumerable<IFilter<Team>> filters )
    {
        IQueryable<Team> query = _dbContext.Set<Team>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public override async Task<Team?> Get( int id )
    {
        return await _dbContext.Set<Team>()
            .Include( t => t.Members )
            .Include( t => t.Department )
            .FirstOrDefaultAsync( t => t.Id == id );
    }

    public async Task<List<Team>> GetFilteredTeams( IEnumerable<IFilter<Team>> filters )
    {
        IQueryable<Team> query = _dbContext.Set<Team>()
            .Include( t => t.Members ).ThenInclude( u => u.Roles )
            .Include( t => t.Department )
            .Include( t => t.Leader ).ThenInclude( l => l.ContactInfo );

        query = query.OrderByDescending( t => t.Id );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }

    public async Task<List<Team>> GetTeamsLookup( IEnumerable<IFilter<Team>> filters )
    {
        IQueryable<Team> query = _dbContext.Set<Team>()
            .OrderBy( t => t.Name );

        query = query.OrderByDescending( t => t.Id );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }
}
