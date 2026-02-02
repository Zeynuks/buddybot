using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Countries.Commands.UpdateCountry;
public class UpdateCountryCommandHandler(
    ICountryRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCountryCommand> logger )
    : CommandBaseHandlerWithResult<UpdateCountryCommand, Country>( logger )
{
    protected override async Task<Result<Country>> HandleImplAsync( UpdateCountryCommand command )
    {
        Country? country = await repository.Get( command.Id );
        if ( country == null )
        {
            return Result<Country>.FromError( $"Страна с ID - {command.Id} не найдена." );
        }

        if ( !string.IsNullOrWhiteSpace( command.Name ) )
        {
            country.Name = command.Name;
        }
        await unitOfWork.CommitAsync();

        return Result<Country>.FromSuccess( country );
    }
}
