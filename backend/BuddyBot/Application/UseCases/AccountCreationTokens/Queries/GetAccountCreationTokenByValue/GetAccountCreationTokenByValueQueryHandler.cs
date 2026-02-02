using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokenByValue;
public class GetAccountCreationTokenByValueQueryHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    ILogger<GetAccountCreationTokenByValueQuery> logger )
    : QueryBaseHandler<AccountCreationToken, GetAccountCreationTokenByValueQuery>( logger )
{
    protected override async Task<Result<AccountCreationToken>> HandleImplAsync( GetAccountCreationTokenByValueQuery query )
    {
        AccountCreationToken? token = await accountCreationTokenRepository.GetByTokenValue( query.TokenValue );
        if ( token == null )
        {
            return Result<AccountCreationToken>.FromError( $"Приглашение с токеном {query.TokenValue} не найдено." );
        }

        return Result<AccountCreationToken>.FromSuccess( token );
    }
}