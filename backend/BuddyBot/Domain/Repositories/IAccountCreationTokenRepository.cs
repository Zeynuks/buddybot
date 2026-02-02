using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface IAccountCreationTokenRepository : IBaseRepository<AccountCreationToken>
{
    Task<int> CountFilteredTokens( IEnumerable<IFilter<AccountCreationToken>> filters );
    Task<List<AccountCreationToken>> GetFilteredTokens( IEnumerable<IFilter<AccountCreationToken>> filters );
    Task<AccountCreationToken?> GetByTokenValue( Guid tokenValue );
    Task<List<AccountCreationToken>> GetExpiredNotMarkedAsExpired( DateTime utcNow );
    Task<AccountCreationToken?> GetByUserId( int userId );
}
