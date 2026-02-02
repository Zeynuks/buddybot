using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.AccountCreationTokens.Commands.CreateCandidateInvite;
using Application.UseCases.AccountCreationTokens.Commands.CreateHRInvite;
using Application.UseCases.AccountCreationTokens.Commands.DeleteToken;
using Application.UseCases.AccountCreationTokens.Commands.RevokeToken;
using Application.UseCases.AccountCreationTokens.Commands.UpdateToken;
using Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokenByValue;
using Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokens;
using AutoMapper;
using Contracts.AccountCreationTokenDtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/account-creation-tokens" )]
[Authorize]

public class AccountCreationTokensController : ControllerBase
{
    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenView ) )]
    public async Task<ActionResult<PagedResult<AccountCreationTokenListDto>>> GetAccountCreationTokens(
        [FromQuery] GetAccountCreationTokensQuery query,
        [FromServices] IQueryHandler<PagedResult<AccountCreationToken>, GetAccountCreationTokensQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        Result<PagedResult<AccountCreationToken>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<AccountCreationTokenListDto>
        {
            Items = mapper.Map<List<AccountCreationTokenListDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "{tokenValue:guid}" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenView ) )]
    public async Task<ActionResult<AccountCreationTokenDetailDto>> GetByTokenValue(
        [FromRoute] Guid tokenValue,
        [FromServices] IQueryHandler<AccountCreationToken, GetAccountCreationTokenByValueQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        GetAccountCreationTokenByValueQuery query = new GetAccountCreationTokenByValueQuery
        {
            TokenValue = tokenValue
        };

        Result<AccountCreationToken> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        AccountCreationTokenDetailDto dto = mapper.Map<AccountCreationTokenDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost( "candidate" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenCreateCandidate ) )]
    public async Task<ActionResult<AccountCreationTokenDetailDto>> CreateCandidateInvite(
            [FromBody] CreateCandidateInviteCommand command,
            [FromServices] ICommandHandlerWithResult<CreateCandidateInviteCommand, AccountCreationToken> commandHandler,
            [FromServices] IMapper mapper )
    {
        Result<AccountCreationToken> result = await commandHandler.HandleAsync( command );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        AccountCreationTokenDetailDto dto = mapper.Map<AccountCreationTokenDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost( "hr" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenCreateHr ) )]
    public async Task<ActionResult<AccountCreationTokenDetailDto>> CreateHRInvite(
            [FromBody] CreateHRInviteCommand command,
            [FromServices] ICommandHandlerWithResult<CreateHRInviteCommand, AccountCreationToken> commandHandler,
            [FromServices] IMapper mapper )
    {
        Result<AccountCreationToken> result = await commandHandler.HandleAsync( command );
        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        AccountCreationTokenDetailDto dto = mapper.Map<AccountCreationTokenDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPost( "{tokenValue}/revoke" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenRevoke ) )]
    public async Task<ActionResult<AccountCreationTokenDetailDto>> RevokeToken(
        [FromRoute] Guid tokenValue,
        [FromServices] ICommandHandlerWithResult<RevokeTokenCommand, AccountCreationToken> commandHandler,
        [FromServices] IMapper mapper )
    {
        RevokeTokenCommand command = new RevokeTokenCommand
        {
            TokenValue = tokenValue
        };

        Result<AccountCreationToken> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        AccountCreationTokenDetailDto dto = mapper.Map<AccountCreationTokenDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpPut( "{tokenValue}" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenUpdate ) )]
    public async Task<ActionResult<AccountCreationTokenDetailDto>> UpdateToken(
    [FromRoute] Guid tokenValue,
    [FromBody] UpdateTokenCommand command,
    [FromServices] ICommandHandlerWithResult<UpdateTokenCommand, AccountCreationToken> commandHandler,
    [FromServices] IMapper mapper )
    {
        command.TokenValue = tokenValue;

        Result<AccountCreationToken> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        AccountCreationTokenDetailDto dto = mapper.Map<AccountCreationTokenDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{tokenValue}" )]
    [Authorize( Policy = nameof( PermissionName.AccountCreationTokenDelete ) )]
    public async Task<ActionResult<string>> DeleteToken(
        [FromRoute] Guid tokenValue,
        [FromServices] ICommandHandlerWithResult<DeleteTokenCommand, string> commandHandler )
    {
        DeleteTokenCommand command = new DeleteTokenCommand
        {
            TokenValue = tokenValue,
        };

        Result<string> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        return Ok( result.Value );
    }
}
