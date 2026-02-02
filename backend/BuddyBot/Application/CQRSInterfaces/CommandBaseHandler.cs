using Application.Results;
using Microsoft.Extensions.Logging;

namespace Application.CQRSInterfaces;
public abstract class CommandBaseHandler<TCommand> ( ILogger<TCommand> logger )
    : ICommandHandler<TCommand> where TCommand : class
{
    public async Task<Result> HandleAsync( TCommand command )
    {
        try
        {
            await HandleImplAsync( command );
            return Result.Success;
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Error handling command of type {CommandType}.", typeof( TCommand ).Name );
            await CleanupOnFailureAsync( command );
            return Result.FromError( ex.Message );
        }
    }

    protected abstract Task<Result> HandleImplAsync( TCommand command );
    protected virtual Task CleanupOnFailureAsync( TCommand command )
    {
        return Task.CompletedTask;
    }
}
