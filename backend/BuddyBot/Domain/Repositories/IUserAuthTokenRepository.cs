using Domain.Entities;

namespace Domain.Repositories;
public interface IUserAuthTokenRepository : IBaseRepository<UserAuthToken>
{
    Task<UserAuthToken?> GetByUserId( int userId );
    Task<UserAuthToken?> GetByRefreshToken( string refreshToken );
    Task Delete( UserAuthToken entity );
}
