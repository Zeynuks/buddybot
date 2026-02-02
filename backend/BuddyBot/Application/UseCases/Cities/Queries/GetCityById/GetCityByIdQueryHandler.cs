using Application.CQRSInterfaces;
using Application.Results;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Cities.Queries.GetCityById;
public class GetCityByIdQueryHandler( 
    ICityRepository cityRepository,
    ILogger<GetCityByIdQuery> logger )
    : QueryBaseHandler<City, GetCityByIdQuery>(logger)
{
    protected override async Task<Result<City>> HandleImplAsync( GetCityByIdQuery query )
    {
        City? city = await cityRepository.Get( query.Id );
        if ( city == null )
        {
            return Result<City>.FromError( $"Город с ID {query.Id} не найден." );
        }

        return Result<City>.FromSuccess( city );
    }
}