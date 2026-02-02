using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;
public interface IRoleRepository : IBaseRepository<Role>
{
    Task<List<Permission>> GetPermissions( RoleName roleName );
    Task Delete( RoleName roleName );
    Task<Role?> Get( RoleName roleName );
}
