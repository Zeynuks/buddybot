using Application.CQRSInterfaces;
using Application.UseCases.Countries.Commands.CreateCountry;
using Application.UseCases.Countries.Commands.DeleteCountry;
using Application.UseCases.Countries.Commands.UpdateCountry;
using Application.UseCases.Countries.Queries.GetCountries;
using Application.UseCases.Countries.Queries.GetCountryById;
using Microsoft.AspNetCore.Mvc;
using Application.Results;
using AutoMapper;
using Application.UseCases.Countries.Queries.GetCountriesLookup;
using Domain.Entities;
using Contracts.CountryDtos;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/countries" )]
[Authorize]

public class CountriesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<CountryListDto>>> GetCountries(
            [FromQuery] GetCountriesQuery query,
            [FromServices] IQueryHandler<PagedResult<Country>, GetCountriesQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        Result<PagedResult<Country>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<CountryListDto>
        {
            Items = mapper.Map<List<CountryListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<CountryDetailDto>> GetCountryById(
            [FromRoute] int id,
            [FromServices] IQueryHandler<Country, GetCountryByIdQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetCountryByIdQuery query = new GetCountryByIdQuery
        {
            Id = id,
        };

        Result<Country> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        CountryDetailDto dto = mapper.Map<CountryDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost]
    public async Task<ActionResult<CountryListDto>> CreateCountry(
            [FromBody] CreateCountryCommand command,
            [FromServices] ICommandHandlerWithResult<CreateCountryCommand, Country> commandHandler,
            [FromServices] IMapper mapper )
    {
        Result<Country> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        CountryListDto dto = mapper.Map<CountryListDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{id}" )]
    public async Task<ActionResult<CountryListDto>> UpdateCountry(
            [FromRoute] int id,
            [FromBody] UpdateCountryCommand command,
            [FromServices] ICommandHandlerWithResult<UpdateCountryCommand, Country> commandHandler,
            [FromServices] IMapper mapper )
    {
        command.Id = id;

        Result<Country> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        CountryListDto dto = mapper.Map<CountryListDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    public async Task<ActionResult<string>> DeleteCountry(
            [FromRoute] int id,
            [FromServices] ICommandHandlerWithResult<DeleteCountryCommand, string> commandHandler )
    {
        DeleteCountryCommand command = new DeleteCountryCommand
        {
            Id = id,
        };

        Result<string> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        return Ok( result.Value );
    }

    [HttpGet( "lookup" )]
    public async Task<ActionResult<List<CountryLookupDto>>> GetCountriesLookup(
    [FromServices] IQueryHandler<List<Country>, GetCountriesLookupQuery> queryHandler,
    [FromServices] IMapper mapper )
    {
        GetCountriesLookupQuery query = new GetCountriesLookupQuery();
        Result<List<Country>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        List<CountryLookupDto> dtos = mapper.Map<List<CountryLookupDto>>( result.Value );
        return Ok( dtos );
    }
}
