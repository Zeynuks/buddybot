using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Cities.Commands.DeleteCity;
public class DeleteCityCommandHandler(
    ICityRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCityCommand> logger )
    : CommandBaseHandlerWithResult<DeleteCityCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteCityCommand command )
    {
        City? city = await repository.Get( command.Id );
        if ( city == null )
        {
            return Result<string>.FromError( $"Город с ID {command.Id} не найден." );
        }

        await repository.Delete( command.Id );
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Город с ID {command.Id} был успешно удалён." );
    }
}
