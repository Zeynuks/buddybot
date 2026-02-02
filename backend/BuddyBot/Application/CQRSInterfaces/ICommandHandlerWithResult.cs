using Application.Results;

namespace Application.CQRSInterfaces;
public interface ICommandHandlerWithResult<TCommand, TResult> where TCommand : class
{
    Task<Result<TResult>> HandleAsync( TCommand command );
}