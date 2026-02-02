using Application.CQRSInterfaces;
using Application.UseCases.Departments.Commands.CreateDepartment;
using Application.UseCases.Departments.Commands.DeleteDepartment;
using Application.UseCases.Departments.Commands.UpdateDepartment;
using Application.UseCases.Departments.Queries.GetDepartmentById;
using Application.UseCases.Departments.Queries.GetDepartments;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.Results;
using AutoMapper;
using Application.UseCases.Departments.Queries.GetDepartmentsLookup;
using Contracts.DepartmentDtos;
using Microsoft.AspNetCore.Authorization;
using Domain.Enums;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/departments" )]
[Authorize]

public class DepartmentsController : ControllerBase
{
    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.DepartmentView ) )]
    public async Task<ActionResult<PagedResult<DepartmentListDto>>> GetDepartments(
            [FromQuery] GetDepartmentsQuery query,
            [FromServices] IQueryHandler<PagedResult<Department>, GetDepartmentsQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        Result<PagedResult<Department>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<DepartmentListDto>
        {
            Items = mapper.Map<List<DepartmentListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.DepartmentView ) )]
    public async Task<ActionResult<DepartmentDetailDto>> GetDepartmentById(
            [FromRoute] int id,
            [FromServices] IQueryHandler<Department, GetDepartmentByIdQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetDepartmentByIdQuery query = new GetDepartmentByIdQuery
        {
            Id = id,
        };

        Result<Department> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        DepartmentDetailDto dto = mapper.Map<DepartmentDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost]
    [Authorize( Policy = nameof( PermissionName.DepartmentCreate ) )]
    public async Task<ActionResult<DepartmentDetailDto>> CreateDepartment(
            [FromBody] CreateDepartmentCommand command,
            [FromServices] ICommandHandlerWithResult<CreateDepartmentCommand, Department> commandHandler,
            [FromServices] IMapper mapper )
    {
        Result<Department> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        DepartmentDetailDto dto = mapper.Map<DepartmentDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.DepartmentUpdate ) )]
    public async Task<ActionResult<DepartmentListDto>> UpdateDepartment(
            [FromRoute] int id,
            [FromBody] UpdateDepartmentCommand command,
            [FromServices] ICommandHandlerWithResult<UpdateDepartmentCommand, Department> commandHandler,
            [FromServices] IMapper mapper )
    {
        command.Id = id;

        Result<Department> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        DepartmentListDto dto = mapper.Map<DepartmentListDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.DepartmentDelete ) )]
    public async Task<ActionResult<string>> DeleteDepartment(
            [FromRoute] int id,
            [FromServices] ICommandHandlerWithResult<DeleteDepartmentCommand, string> commandHandler )
    {
        DeleteDepartmentCommand command = new DeleteDepartmentCommand
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
    [Authorize( Policy = nameof( PermissionName.DepartmentView ) )]
    public async Task<ActionResult<List<DepartmentLookupDto>>> GetDepartmentsLookup(
            [FromServices] IQueryHandler<List<Department>, GetDepartmentsLookupQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetDepartmentsLookupQuery query = new GetDepartmentsLookupQuery();
        Result<List<Department>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        List<DepartmentLookupDto> dtos = mapper.Map<List<DepartmentLookupDto>>( result.Value );
        return Ok( dtos );
    }
}
