using Application.CQRSInterfaces;
using Application.UseCases.Cities.Commands.CreateCity;
using Application.UseCases.Cities.Commands.DeleteCity;
using Application.UseCases.Cities.Commands.UpdateCity;
using Application.UseCases.Cities.Queries.GetCities;
using Application.UseCases.Cities.Queries.GetCityById;
using Application.Results;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.UseCases.Cities.Queries.GetCitiesLookup;
using Domain.Entities;
using Contracts.CityDtos;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/cities" )]
//[Authorize]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<CityListDto>>> GetCities(
            [FromQuery] GetCitiesQuery query,
            [FromServices] IQueryHandler<PagedResult<City>, GetCitiesQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        Result<PagedResult<City>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<CityListDto>
        {
            Items = mapper.Map<List<CityListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<CityDetailDto>> GetCityById(
            [FromRoute] int id,
            [FromServices] IQueryHandler<City, GetCityByIdQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetCityByIdQuery query = new GetCityByIdQuery
        {
            Id = id,
        };

        Result<City> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        CityDetailDto dto = mapper.Map<CityDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost]
    public async Task<ActionResult<CityListDto>> CreateCity(
            [FromBody] CreateCityCommand command,
            [FromServices] ICommandHandlerWithResult<CreateCityCommand, City> commandHandler,
            [FromServices] IMapper mapper )
    {
        Result<City> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        CityListDto dto = mapper.Map<CityListDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{id}" )]
    public async Task<ActionResult<CityListDto>> UpdateCity(
            [FromRoute] int id,
            [FromBody] UpdateCityCommand command,
            [FromServices] ICommandHandlerWithResult<UpdateCityCommand, City> commandHandler,
            [FromServices] IMapper mapper )
    {
        command.Id = id;

        Result<City> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        CityListDto dto = mapper.Map<CityListDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    public async Task<ActionResult<string>> DeleteCity(
            [FromRoute] int id,
            [FromServices] ICommandHandlerWithResult<DeleteCityCommand, string> commandHandler )
    {
        DeleteCityCommand command = new DeleteCityCommand
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
    public async Task<ActionResult<List<CityLookupDto>>> GetCitiesLookup(
        [FromQuery] GetCitiesLookupQuery query,
        [FromServices] IQueryHandler<List<City>, GetCitiesLookupQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        Result<List<City>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        List<CityLookupDto> dtos = mapper.Map<List<CityLookupDto>>( result.Value );
        return Ok( dtos );
    }
}
