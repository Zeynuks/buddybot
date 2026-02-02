using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Foundation.Database;

namespace Infrastructure.Repositories;
public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    public PermissionRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }
}
