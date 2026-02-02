#pragma warning disable CS8602
using Application.Filters;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public override async Task<User?> Get( int id )
    {
        return await _dbContext.Set<User>()
            .Include( u => u.ContactInfo )
            .Include( u => u.Team ).ThenInclude( t => t.Department )
            .Include( u => u.Team ).ThenInclude( t => t.Leader ).ThenInclude( l => l.ContactInfo )
            .Include( u => u.Mentors ).ThenInclude( m => m.ContactInfo )
            .Include( u => u.HRs ).ThenInclude( hr => hr.ContactInfo )
            .Include( u => u.Roles )
            .Include( u => u.CandidateProcesses )
            .Include( u => u.OnboardingAccessRequest )
            .FirstOrDefaultAsync( u => u.Id == id && !u.IsDeleted );
    }

    public async Task<User?> GetByLogin( string login )
    {
        return await _dbContext.Set<User>()
            .Include( u => u.AuthToken )
            .FirstOrDefaultAsync( u => u.Login == login && !u.IsDeleted );
    }

    public async Task<List<User>> GetMentors( int id )
    {
        User? user = await _dbContext.Set<User>()
            .Include( u => u.Mentors )
            .FirstOrDefaultAsync( u => u.Id == id );

        if ( user == null )
        {
            return new List<User>();
        }

        return user.Mentors.Where( m => !m.IsDeleted ).ToList();
    }

    public async Task<List<PermissionName>> GetUserPermissions( int id )
    {
        return await _dbContext.Set<User>()
            .Where( u => u.Id == id )
            .Include( u => u.Roles ).ThenInclude( r => r.Permissions )
            .SelectMany( u => u.Roles )
            .SelectMany( r => r.Permissions )
            .Select( p => p.PermissionName )
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByIds( List<int> ids )
    {
        return await _dbContext.Set<User>()
            .Where( u => ids.Contains( u.Id ) && !u.IsDeleted )
            .Include( u => u.ContactInfo )
            .ToListAsync();
    }

    public async Task<User?> GetByTelegramId( long telegramId )
    {
        return await _dbContext.Set<User>()
            .Include( u => u.ContactInfo )
            .Include( u => u.Team ).ThenInclude( t => t.Department )
            .Include( u => u.Team ).ThenInclude( t => t.Leader ).ThenInclude( l => l.ContactInfo )
            .Include( u => u.HRs ).ThenInclude( hr => hr.ContactInfo )
            .Include( u => u.Mentors ).ThenInclude( m => m.ContactInfo )
            .Include( u => u.Roles )
            .FirstOrDefaultAsync( u => !u.IsDeleted && u.ContactInfo != null && u.ContactInfo.TelegramId == telegramId );
    }

    public async Task<List<RoleName>> GetUserRoles( int id )
    {
        return await _dbContext.Set<User>()
            .Where( u => u.Id == id )
            .SelectMany( u => u.Roles )
            .Select( r => r.RoleName )
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<User>> GetFilteredUsers( IEnumerable<IFilter<User>> filters )
    {
        IQueryable<User> query = _dbContext.Set<User>()
            .Include( u => u.ContactInfo )
            .Include( u => u.Team ).ThenInclude( t => t.Department )
            .Include( u => u.Team ).ThenInclude( t => t.Leader ).ThenInclude( l => l.ContactInfo )
            .Include( u => u.HRs ).ThenInclude( hr => hr.ContactInfo )
            .Include( u => u.Mentors ).ThenInclude( m => m.ContactInfo )
            .Include( u => u.CandidateProcesses )
            .Include( u => u.Roles );

        query = query.OrderByDescending( u => u.CreatedAtUtc );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }

    public async Task<int> CountFilteredUsers( IEnumerable<IFilter<User>> filters )
    {
        IQueryable<User> query = _dbContext.Set<User>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }
}
#pragma warning restore CS8602