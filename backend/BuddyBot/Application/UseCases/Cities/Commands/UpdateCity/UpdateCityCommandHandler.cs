using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Cities.Commands.UpdateCity;
public class UpdateCityCommandHandler(
    ICityRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCityCommand> logger )
    : CommandBaseHandlerWithResult<UpdateCityCommand, City>( logger )
{
    protected override async Task<Result<City>> HandleImplAsync( UpdateCityCommand command )
    {
        City? city = await repository.Get( command.Id );
        if ( city == null )
        {
            return Result<City>.FromError( $"Город с ID {command.Id} не найден." );
        }

        if ( !string.IsNullOrWhiteSpace( command.Name ) )
        {
            city.Name = command.Name;
        }
        city.CountryId = command.CountryId;

        await unitOfWork.CommitAsync();
        return Result<City>.FromSuccess( city );
    }
}
