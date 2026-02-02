using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.Teams.Commands.CreateTeam;
using Application.UseCases.Teams.Commands.DeleteTeam;
using Application.UseCases.Teams.Commands.UpdateTeam;
using Application.UseCases.Teams.Queries.GetTeamById;
using Application.UseCases.Teams.Queries.GetTeams;
using Application.UseCases.Teams.Queries.GetTeamsLookup;
using AutoMapper;
using Contracts.TeamDtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/teams" )]
[Authorize]

public class TeamsController : ControllerBase
{
    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.TeamView ) )]
    public async Task<ActionResult<PagedResult<TeamListDto>>> GetTeams(
            [FromQuery] GetTeamsQuery query,
            [FromServices] IQueryHandler<PagedResult<Team>, GetTeamsQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        Result<PagedResult<Team>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<TeamListDto>
        {
            Items = mapper.Map<List<TeamListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.TeamView ) )]
    public async Task<ActionResult<TeamListDto>> GetTeamById(
            [FromRoute] int id,
            [FromServices] IQueryHandler<Team, GetTeamByIdQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetTeamByIdQuery query = new GetTeamByIdQuery
        {
            Id = id,
        };

        Result<Team> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        TeamListDto dto = mapper.Map<TeamListDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost]
    [Authorize( Policy = nameof( PermissionName.TeamCreate ) )]
    public async Task<ActionResult<TeamListDto>> CreateTeam(
        [FromBody] CreateTeamCommand command,
        [FromServices] ICommandHandlerWithResult<CreateTeamCommand, Team> commandHandler,
        [FromServices] IMapper mapper )
    {
        Result<Team> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        TeamListDto dto = mapper.Map<TeamListDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.TeamUpdate ) )]
    public async Task<ActionResult<TeamListDto>> UpdateTeam(
            [FromRoute] int id,
            [FromBody] UpdateTeamCommand command,
            [FromServices] ICommandHandlerWithResult<UpdateTeamCommand, Team> commandHandler,
            [FromServices] IMapper mapper )
    {
        command.Id = id;

        Result<Team> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        TeamListDto dto = mapper.Map<TeamListDto>( result.Value );
        return Ok( dto );
    }

    [HttpGet( "lookup" )]
    [Authorize( Policy = nameof( PermissionName.TeamView ) )]
    public async Task<ActionResult<List<TeamLookupDto>>> GetTeamsLookup(
        [FromQuery] GetTeamsLookupQuery query,
        [FromServices] IQueryHandler<List<Team>, GetTeamsLookupQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        Result<List<Team>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        List<TeamLookupDto> dtos = mapper.Map<List<TeamLookupDto>>( result.Value );
        return Ok( dtos );
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.TeamDelete ) )]
    public async Task<ActionResult<string>> DeleteTeam(
    [FromRoute] int id,
    [FromServices] ICommandHandlerWithResult<DeleteTeamCommand, string> commandHandler )
    {
        DeleteTeamCommand command = new DeleteTeamCommand
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
}
