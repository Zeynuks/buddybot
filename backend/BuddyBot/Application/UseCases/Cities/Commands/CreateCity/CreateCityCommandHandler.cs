using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Cities.Commands.CreateCity;
public class CreateCityCommandHandler(
    ICityRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateCityCommand> logger )
    : CommandBaseHandlerWithResult<CreateCityCommand, City> (logger)
{
    protected override async Task<Result<City>> HandleImplAsync( CreateCityCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Name ) )
        {
            return Result<City>.FromError( "Имя города не может быть пустым." );
        }

        City? city = new City( command.CountryId, command.Name );

        repository.Add( city );
        await unitOfWork.CommitAsync();

        return Result<City>.FromSuccess( city );
    }
}
