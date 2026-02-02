using Application.CQRSInterfaces;
using Application.Results;
using Application.Filters.Countries;
using Domain.Filters;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Countries.Queries.GetCountries;
public class GetCountriesQueryHandler( 
    ICountryRepository countryRepository,
    ILogger<GetCountriesQuery> logger )
    : QueryBaseHandler<PagedResult<Country>, GetCountriesQuery>(logger)
{
    protected override async Task<Result<PagedResult<Country>>> HandleImplAsync( GetCountriesQuery query )
    {
        List<IFilter<Country>> filters = new List<IFilter<Country>>
        {
            new CountrySearchFilter { SearchTerm = query.SearchTerm },
            new CountryPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize }
        };

        int totalCount = await countryRepository.CountFilteredCountries( filters );
        filters.Add( new CountryPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<Country> countries = await countryRepository.GetFilteredCountries( filters );

        return Result<PagedResult<Country>>.FromSuccess( new PagedResult<Country>
        {
            Items = countries,
            TotalCount = totalCount
        } );
    }
}
