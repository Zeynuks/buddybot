using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Countries.Commands.DeleteCountry;
public class DeleteCountryCommandHandler(
    ICountryRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCountryCommand> logger )
    : CommandBaseHandlerWithResult<DeleteCountryCommand, string>(logger)
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteCountryCommand command )
    {
        Country? country = await repository.Get( command.Id );
        if ( country == null )
        {
            return Result<string>.FromError( $"Страна с ID - {command.Id} не найдена." );
        }

        await repository.Delete( command.Id );
        await unitOfWork.CommitAsync();

         return Result<string>.FromSuccess($"Страна с ID {command.Id} была успешно удалена.");
    }
}

