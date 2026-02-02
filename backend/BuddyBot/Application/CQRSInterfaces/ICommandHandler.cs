using Application.Results;

namespace Application.CQRSInterfaces;
public interface ICommandHandler<TCommand> where TCommand : class
{
    Task<Result> HandleAsync( TCommand command );
}
