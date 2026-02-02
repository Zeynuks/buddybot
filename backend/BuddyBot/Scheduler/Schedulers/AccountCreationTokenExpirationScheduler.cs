using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.AccountCreationTokens.Commands.MarkTokenAsExpired;
using Application.UseCases.AccountCreationTokens.Queries.GetExpiredTokens;
using Domain.Entities;

namespace Scheduler.Schedulers;
public class AccountCreationTokenExpirationScheduler(
    IQueryHandler<List<AccountCreationToken>, GetExpiredTokensQuery> getExpiredTokensHandler,
    ICommandHandlerWithResult<MarkTokenAsExpiredCommand, AccountCreationToken> markTokenAsExpiredHandler
)
{
    public async Task ProcessExpiredTokens(  )
    {
        var utcNow = DateTime.UtcNow;
        GetExpiredTokensQuery query = new GetExpiredTokensQuery 
        { 
            UtcNow = utcNow 
        };

        Result<List<AccountCreationToken>> result = await getExpiredTokensHandler.HandleAsync( query );

        if ( !result.IsSuccess || result.Value == null )
        {
            return;
        }

        foreach ( AccountCreationToken token in result.Value )
        {
            MarkTokenAsExpiredCommand command = new MarkTokenAsExpiredCommand 
            { 
                TokenValue = token.TokenValue 
            };

            await markTokenAsExpiredHandler.HandleAsync( command );
        }
    }
}
