using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserAuthTokenRepository : BaseRepository<UserAuthToken>, IUserAuthTokenRepository
{
    public UserAuthTokenRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<UserAuthToken?> GetByRefreshToken( string refreshToken )
    {
        return await _dbContext.Set<UserAuthToken>()
            .FirstOrDefaultAsync( t => t.RefreshToken == refreshToken );
    }

    public async Task<UserAuthToken?> GetByUserId( int userId )
    {
        return await _dbContext.Set<UserAuthToken>().FindAsync( userId );
    }

    public async Task Delete( UserAuthToken entity )
    {
        await Delete( entity.UserId );
    }
}
