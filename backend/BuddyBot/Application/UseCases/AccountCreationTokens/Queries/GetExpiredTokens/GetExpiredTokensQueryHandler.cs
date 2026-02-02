using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Queries.GetExpiredTokens;
public class GetExpiredTokensQueryHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    ILogger<GetExpiredTokensQuery> logger )
    : QueryBaseHandler<List<AccountCreationToken>, GetExpiredTokensQuery>( logger )
{
    protected override async Task<Result<List<AccountCreationToken>>> HandleImplAsync( GetExpiredTokensQuery query )
    {
        List<AccountCreationToken> tokens = await accountCreationTokenRepository.GetExpiredNotMarkedAsExpired( query.UtcNow );
        if ( tokens == null || tokens.Count == 0 )
        {
            return Result<List<AccountCreationToken>>.FromError( "Нет пользователей, у которых истекло приглашение." );
        }

        return Result<List<AccountCreationToken>>.FromSuccess( tokens );
    }
}
