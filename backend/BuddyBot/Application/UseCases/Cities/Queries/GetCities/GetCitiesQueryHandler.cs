using Application.CQRSInterfaces;
using Application.Results;
using Application.Filters.Cities;
using Domain.Filters;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;
using Application.Filters.Departments;

namespace Application.UseCases.Cities.Queries.GetCities;
public class GetCitiesQueryHandler( 
    ICityRepository cityRepository,
    ILogger<GetCitiesQuery> logger )
    : QueryBaseHandler<PagedResult<City>, GetCitiesQuery>(logger)
{
    protected override async Task<Result<PagedResult<City>>> HandleImplAsync( GetCitiesQuery query )
    {
        List<IFilter<City>> filters = new List<IFilter<City>>
        {
            new CitySearchFilter { SearchTerm = query.SearchTerm },
            new CityCountryFilter { CountryId = query.CountryId },
        };

        int totalCount = await cityRepository.CountFilteredCities( filters );
        filters.Add( new CityPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<City> cities = await cityRepository.GetFilteredCities( filters );

        return Result<PagedResult<City>>.FromSuccess( new PagedResult<City>
        {
            Items = cities,
            TotalCount = totalCount
        } );
    }
}
