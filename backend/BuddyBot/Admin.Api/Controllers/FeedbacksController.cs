using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.Feedbacks.Commands.CreateFeedback;
using Application.UseCases.Feedbacks.Commands.DeleteFeedback;
using Application.UseCases.Feedbacks.Commands.ExportFeedbacks;
using Application.UseCases.Feedbacks.Queries.GetFeedbacks;
using AutoMapper;
using Contracts.FeedbackDtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/feedbacks" )]
[Authorize]
public class FeedbacksController : ControllerBase
{
    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.FeedbackView ) )]
    public async Task<ActionResult<PagedResult<FeedbackListDto>>> GetFeedbacks(
        [FromQuery] GetFeedbacksQuery query,
        [FromServices] IQueryHandler<PagedResult<Feedback>, GetFeedbacksQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        Result<PagedResult<Feedback>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<FeedbackListDto>
        {
            Items = mapper.Map<List<FeedbackListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackListDto>> CreateFeedback(
    [FromBody] CreateFeedbackCommand command,
    [FromServices] ICommandHandlerWithResult<CreateFeedbackCommand, Feedback> commandHandler,
    [FromServices] IMapper mapper )
    {
        Result<Feedback> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        FeedbackListDto dto = mapper.Map<FeedbackListDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.FeedbackDelete ) )]
    public async Task<ActionResult<string>> DeleteFeedback(
        [FromRoute] int id,
        [FromServices] ICommandHandlerWithResult<DeleteFeedbackCommand, string> commandHandler )
    {
        DeleteFeedbackCommand command = new DeleteFeedbackCommand
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

    [HttpGet( "export" )]
    [Authorize( Policy = nameof( PermissionName.FeedbackView ) )]
    public async Task<IActionResult> ExportFeedbacks(
        [FromServices] ICommandHandlerWithResult<ExportFeedbacksCommand, byte[]> commandHandler )
    {
        ExportFeedbacksCommand command = new ExportFeedbacksCommand();
        Result<byte[]> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return File( result.Value, "text/csv; charset=utf-8", $"feedbacks_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv" );
    }
}
