using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Countries.Commands.CreateCountry;

public class CreateCountryCommandHandler( 
    ICountryRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateCountryCommand> logger ) 
    : CommandBaseHandlerWithResult<CreateCountryCommand, Country>( logger )
{
    protected override async Task<Result<Country>> HandleImplAsync( CreateCountryCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Name ) )
        {
            return Result<Country>.FromError( "Имя страны не может быть пустым." );
        }

        Country country = new Country( command.Name );

        repository.Add( country );
        await unitOfWork.CommitAsync();

        return Result<Country>.FromSuccess( country );
    }
}
