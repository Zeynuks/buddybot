using Application.CQRSInterfaces;
using Application.Filters.Cities;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Cities.Queries.GetCitiesLookup;
public class GetCitiesLookupQueryHandler( 
    ICityRepository cityRepository,
    ILogger<GetCitiesLookupQuery> logger )
    : QueryBaseHandler<List<City>, GetCitiesLookupQuery>( logger )
{
    protected override async Task<Result<List<City>>> HandleImplAsync( GetCitiesLookupQuery query )
    {
        List<IFilter<City>> filters = new List<IFilter<City>>
        {
            new CityCountryFilter { CountryId = query.CountryId },
        };

        List<City> cities = await cityRepository.GetCitiesLookup( filters );
        return Result<List<City>>.FromSuccess( cities );
    }
}
