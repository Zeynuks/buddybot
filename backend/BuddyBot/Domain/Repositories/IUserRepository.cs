using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Domain.Repositories;
public interface IUserRepository : IBaseRepository<User>
{
    Task<List<PermissionName>> GetUserPermissions( int id );
    Task<List<RoleName>> GetUserRoles( int id );
    Task<User?> GetByLogin( string login );
    Task<User?> GetByTelegramId( long telegramId );
    Task<List<User>> GetUsersByIds( List<int> ids );
    Task<List<User>> GetMentors( int id );
    Task<int> CountFilteredUsers( IEnumerable<IFilter<User>> filters );
    Task<List<User>> GetFilteredUsers( IEnumerable<IFilter<User>> filters );

}
