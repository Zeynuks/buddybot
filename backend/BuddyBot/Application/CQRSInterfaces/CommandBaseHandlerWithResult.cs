using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.CQRSInterfaces;
public abstract class CommandBaseHandlerWithResult<TCommand, TResult>( ILogger<TCommand> logger )
    : ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    public virtual async Task<Result<TResult>> HandleAsync( TCommand command )
    {
        try
        {
            return await HandleImplAsync( command );
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Error handling command of type {CommandType}.", typeof( TCommand ).Name );
            await CleanupOnFailureAsync( command );
            return Result<TResult>.FromError( ex.Message );
        }
    }

    protected abstract Task<Result<TResult>> HandleImplAsync( TCommand command );
    protected virtual Task CleanupOnFailureAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
