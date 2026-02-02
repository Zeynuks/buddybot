using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Countries.Queries.GetCountriesLookup;
public class GetCountriesLookupQueryHandler( 
    ICountryRepository countryRepository,
    ILogger<GetCountriesLookupQuery> logger )
    : QueryBaseHandler<List<Country>, GetCountriesLookupQuery>( logger )
{
    protected override async Task<Result<List<Country>>> HandleImplAsync( GetCountriesLookupQuery query )
    {
        List<Country> countries = await countryRepository.GetCountriesLookup();
        return Result<List<Country>>.FromSuccess( countries );
    }
}