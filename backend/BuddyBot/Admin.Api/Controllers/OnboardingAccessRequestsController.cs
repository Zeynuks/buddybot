using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.OnboardingAccessRequests.Commands.ConfirmOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.CreateOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.DeleteOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.RejectOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.RestoreOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Commands.UpdateOnboardingAccessRequest;
using Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequestByCandidateId;
using Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequests;
using AutoMapper;
using Contracts.OnboardingAccessRequestDtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/onboarding-access-requests" )]
[Authorize]

public class OnboardingAccessRequestsController : ControllerBase
{
    [HttpPost]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestCreate ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> CreateOnboardingAccessRequest(
    [FromBody] CreateOnboardingAccessRequestCommand command,
    [FromServices] ICommandHandlerWithResult<CreateOnboardingAccessRequestCommand, OnboardingAccessRequest> commandHandler,
    [FromServices] IMapper mapper )
    {
        Result<OnboardingAccessRequest> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestView ) )]
    public async Task<ActionResult<PagedResult<OnboardingAccessRequestListDto>>> GetOnboardingAccessRequests(
    [FromQuery] GetOnboardingAccessRequestsQuery query,
    [FromServices] IQueryHandler<PagedResult<OnboardingAccessRequest>, GetOnboardingAccessRequestsQuery> queryHandler,
    [FromServices] IMapper mapper )
    {
        Result<PagedResult<OnboardingAccessRequest>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<OnboardingAccessRequestListDto>
        {
            Items = mapper.Map<List<OnboardingAccessRequestListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{candidateId}" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestView ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> GetOnboardingAccessRequestByCandidateId(
        [FromRoute] int candidateId,
        [FromServices] IQueryHandler<OnboardingAccessRequest, GetOnboardingAccessRequestByCandidateIdQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        GetOnboardingAccessRequestByCandidateIdQuery query = new GetOnboardingAccessRequestByCandidateIdQuery
        {
            CandidateId = candidateId,
        };

        Result<OnboardingAccessRequest> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{candidateId}/confirm" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestConfirm ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> ConfirmOnboardingAccessRequest(
            [FromRoute] int candidateId,
            [FromBody] ConfirmOnboardingAccessRequestCommand command,
            [FromServices] ICommandHandlerWithResult<ConfirmOnboardingAccessRequestCommand, OnboardingAccessRequest> commandHandler,
            [FromServices] IMapper mapper )
    {
        command.CandidateId = candidateId;

        Result<OnboardingAccessRequest> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{candidateId}/reject" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestReject ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> RejectOnboardingAccessRequest(
            [FromRoute] int candidateId,
            [FromServices] ICommandHandlerWithResult<RejectOnboardingAccessRequestCommand, OnboardingAccessRequest> commandHandler,
            [FromServices] IMapper mapper )
    {
        RejectOnboardingAccessRequestCommand command = new RejectOnboardingAccessRequestCommand
        {
            CandidateId = candidateId
        };

        Result<OnboardingAccessRequest> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{candidateId}" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestUpdate ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> UpdateOnboardingAccessRequest(
        [FromRoute] int candidateId,
        [FromBody] UpdateOnboardingAccessRequestCommand command,
        [FromServices] ICommandHandlerWithResult<UpdateOnboardingAccessRequestCommand, OnboardingAccessRequest> commandHandler,
        [FromServices] IMapper mapper )
    {
        command.CandidateId = candidateId;

        Result<OnboardingAccessRequest> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestDelete ) )]
    public async Task<ActionResult<string>> DeleteOnboardingAccessRequest(
        [FromRoute] int id,
        [FromServices] ICommandHandlerWithResult<DeleteOnboardingAccessRequestCommand, string> commandHandler )
    {
        DeleteOnboardingAccessRequestCommand command = new DeleteOnboardingAccessRequestCommand
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

    [HttpPut( "{candidateId}/restore" )]
    [Authorize( Policy = nameof( PermissionName.OnboardingAccessRequestRestore ) )]
    public async Task<ActionResult<OnboardingAccessRequestDetailDto>> RestoreOnboardingAccessRequest(
    [FromRoute] int candidateId,
    [FromServices] ICommandHandlerWithResult<RestoreOnboardingAccessRequestCommand, OnboardingAccessRequest> commandHandler,
    [FromServices] IMapper mapper )
    {
        RestoreOnboardingAccessRequestCommand command = new RestoreOnboardingAccessRequestCommand
        {
            CandidateId = candidateId
        };

        Result<OnboardingAccessRequest> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        OnboardingAccessRequestDetailDto dto = mapper.Map<OnboardingAccessRequestDetailDto>( result.Value );
        return Ok( dto );
    }
}