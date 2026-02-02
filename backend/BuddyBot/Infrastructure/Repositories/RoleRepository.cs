using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task Delete( RoleName roleName )
    {
        Role? role = await _dbContext.Set<Role>().FindAsync( roleName );
        if ( role != null )
        {
            _dbContext.Set<Role>().Remove( role );
        }
    }

    public async Task<List<Permission>> GetPermissions( RoleName roleName )
    {
        Role? role = await _dbContext.Set<Role>()
            .Include( r => r.Permissions )
            .FirstOrDefaultAsync( r => r.RoleName == roleName );

        if ( role == null )
        {
            return new List<Permission>();
        }

        return role.Permissions;
    }

    public async Task<Role?> Get( RoleName roleName )
    {
        return await _dbContext.Set<Role>()
            .Include( r => r.Permissions )
            .FirstOrDefaultAsync( r => r.RoleName == roleName );
    }
}
