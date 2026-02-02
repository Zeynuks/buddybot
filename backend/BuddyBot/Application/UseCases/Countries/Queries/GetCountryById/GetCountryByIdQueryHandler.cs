using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Countries.Queries.GetCountryById;
public class GetCountryByIdQueryHandler( 
    ICountryRepository countryRepository,
    ILogger<GetCountryByIdQuery> logger )
    : QueryBaseHandler<Country, GetCountryByIdQuery>(logger)
{
    protected override async Task<Result<Country>> HandleImplAsync( GetCountryByIdQuery query )
    {
        Country? country = await countryRepository.Get( query.Id );
        if ( country == null )
        {
            return Result<Country>.FromError( "Страна не найдена." );
        }

        return Result<Country>.FromSuccess( country );
    }
}
