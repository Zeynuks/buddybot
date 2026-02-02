using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.CQRSInterfaces;
public abstract class QueryBaseHandler<TResult, TQuery>( ILogger<TQuery> logger )
    : IQueryHandler<TResult, TQuery>
    where TResult : class
    where TQuery : class
{
    public async Task<Result<TResult>> HandleAsync( TQuery query )
    {
        try
        {
            return await HandleImplAsync( query );
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Error handling command of type {QueryType}.", typeof( TQuery ).Name );
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleImplAsync( TQuery query );
}
