#pragma warning disable CS8602
using Application.Filters;
using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class AccountCreationTokenRepository : BaseRepository<AccountCreationToken>, IAccountCreationTokenRepository
{
    public AccountCreationTokenRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<int> CountFilteredTokens( IEnumerable<IFilter<AccountCreationToken>> filters )
    {
        IQueryable<AccountCreationToken> query = _dbContext.Set<AccountCreationToken>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public async Task<AccountCreationToken?> GetByTokenValue( Guid tokenValue )
    {
        return await _dbContext.Set<AccountCreationToken>()
            .Include( act => act.User ).ThenInclude( u => u.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.Team ).ThenInclude( t => t.Department )
            .Include( act => act.User ).ThenInclude( u => u.Mentors ).ThenInclude( m => m.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.HRs ).ThenInclude( hr => hr.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.Roles )
            .Include( act => act.Creator ).ThenInclude( u => u.ContactInfo )
            .FirstOrDefaultAsync( t => t.TokenValue == tokenValue );
    }

    public async Task<AccountCreationToken?> GetByUserId( int userId )
    {
        return await _dbContext.Set<AccountCreationToken>()
            .FirstOrDefaultAsync( act => act.UserId == userId && !act.IsDeleted );
    }

    public async Task<List<AccountCreationToken>> GetExpiredNotMarkedAsExpired( DateTime utcNow )
    {
        return await _dbContext.Set<AccountCreationToken>()
            .Where( x => x.ExpireDate != null && x.ExpireDate < utcNow && x.Status != AccountCreationTokenStatus.Expired )
            .ToListAsync();
    }

    public async Task<List<AccountCreationToken>> GetFilteredTokens( IEnumerable<IFilter<AccountCreationToken>> filters )
    {
        IQueryable<AccountCreationToken> query = _dbContext.Set<AccountCreationToken>()
            .Include( act => act.User ).ThenInclude( u => u.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.HRs ).ThenInclude( hr => hr.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.Mentors ).ThenInclude( m => m.ContactInfo )
            .Include( act => act.User ).ThenInclude( u => u.Team ).ThenInclude ( t => t.Department )
            .Include( act => act.User ).ThenInclude( u => u.Roles )
            .Include( act => act.Creator ).ThenInclude( u => u.ContactInfo );

        query = query.OrderByDescending( act => act.IssuedAtUtc );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }
}
#pragma warning restore CS8602