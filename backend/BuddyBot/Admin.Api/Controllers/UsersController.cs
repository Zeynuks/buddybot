using Application;
using Application.CQRSInterfaces;
using Application.Interfaces;
using Application.Results;
using Application.UseCases.Users.Commands.CreateMentor;
using Application.UseCases.Users.Commands.DeleteUser;
using Application.UseCases.Users.Commands.UpdateUser;
using Application.UseCases.Users.Queries.GetUserById;
using Application.UseCases.Users.Queries.GetUsers;
using Application.UseCases.Users.Queries.GetUsersLookup;
using AutoMapper;
using Contracts.UserDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "/api/users" )]

public class UsersController : ControllerBase
{
    [HttpPost( "refresh-token" )]
    public async Task<ActionResult<RefreshTokenRequestDto>> RefreshToken(
        [FromBody] RefreshTokenRequestDto refreshTokenDto,
        [FromServices] IRefreshTokenService refreshTokenService )
    {
        var commandResult = await refreshTokenService.RefreshToken( refreshTokenDto.RefreshToken );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult.Value );
    }
    public record RefreshTokenRequestDto( string RefreshToken );

    [HttpPost( "login" )]
    public async Task<ActionResult<TokenDto>> LoginUser(
        [FromBody] LoginDto loginDto,
        [FromServices] ILoginService service)
    {
        if ( string.IsNullOrWhiteSpace( loginDto.Login ) || string.IsNullOrWhiteSpace( loginDto.Password ) )
        {
            return BadRequest( "Логин и пароль обязательны для входа." );
        }

        var commandResult = await service.Login( loginDto.Login, loginDto.Password );

        if ( !commandResult.IsSuccess )
        {
            return BadRequest( commandResult.Error );
        }

        return Ok( commandResult.Value );
    }

    [HttpPost( "logout" )]
    [Authorize( Policy = nameof( PermissionName.UserLogout ) )]
    public async Task<IActionResult> Logout(
    [FromServices] IUserAuthTokenRepository userAuthTokenRepository,
    [FromServices] IUnitOfWork unitOfWork )
    {
        var userIdClaim = User.FindFirst( "userId" );
        if ( userIdClaim == null || !int.TryParse( userIdClaim.Value, out int userId ) )
            return Unauthorized();

        var token = await userAuthTokenRepository.GetByUserId( userId );
        if ( token != null )
        {
            await userAuthTokenRepository.Delete( token );
            await unitOfWork.CommitAsync();
        }

        return Ok( new { message = "Вы успешно вышли из аккаунта" } );
    }


    [Authorize( Policy = nameof( PermissionName.UserCreateMentor ) )]
    [HttpPost( "create-mentor" )]
    public async Task<ActionResult<UserLookupDto>> CreateMentor(
        [FromBody] CreateMentorCommand command,
        [FromServices] ICommandHandlerWithResult<CreateMentorCommand, User> commandHandler,
        [FromServices] IMapper mapper )
    {
        Result<User> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        UserLookupDto dto = mapper.Map<UserLookupDto>( result.Value );
        return Ok( dto );
    }

    [HttpGet( "me" )]
    [Authorize]
    public async Task<ActionResult<CurrentUserDto>> GetCurrentUser(
        [FromServices] IQueryHandler<User, GetUserByIdQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        var userIdClaim = User.FindFirst( "userId" );

        if ( userIdClaim == null )
        {
            return Unauthorized();
        }

        if ( !int.TryParse( userIdClaim.Value, out int userId ) )
        {
            return Unauthorized();
        }

        GetUserByIdQuery query = new GetUserByIdQuery
        { 
            Id = userId 
        };

        Result<User> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        CurrentUserDto dto = mapper.Map<CurrentUserDto>( result.Value );
        return Ok( dto );
    }

    [HttpGet]
    [Authorize( Policy = nameof( PermissionName.UserView ) )]
    public async Task<ActionResult<PagedResult<UserDetailDto>>> GetUsers(
            [FromQuery] GetUsersQuery query,
            [FromServices] IQueryHandler<PagedResult<User>, GetUsersQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        Result<PagedResult<User>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok( new PagedResult<UserDetailDto>
        {
            Items = mapper.Map<List<UserDetailDto>>( result.Value!.Items ),
            TotalCount = result.Value.TotalCount
        } );
    }

    [HttpGet( "lookup" )]
    [Authorize( Policy = nameof( PermissionName.UserView ) )]
    public async Task<ActionResult<List<UserLookupDto>>> GetUsersLookup(
        [FromQuery] GetUsersLookupQuery query,
        [FromServices] IQueryHandler<List<User>, GetUsersLookupQuery> queryHandler,
        [FromServices] IMapper mapper )
    {
        Result<List<User>> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        List<UserLookupDto> dtos = mapper.Map<List<UserLookupDto>>( result.Value );
        return Ok( dtos );
    }

    [HttpPut( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.UserUpdate ) )]
    public async Task<ActionResult<UserDetailDto>> UpdateUser(
        [FromRoute] int id,
        [FromBody] UpdateUserCommand command,
        [FromServices] ICommandHandlerWithResult<UpdateUserCommand, User> commandHandler,
        [FromServices] IMapper mapper )
    {
        command.Id = id;

        Result<User> result = await commandHandler.HandleAsync( command );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        UserDetailDto dto = mapper.Map<UserDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpGet( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.UserView ) )]
    public async Task<ActionResult<UserDetailDto>> GetUserById(
            [FromRoute] int id,
            [FromServices] IQueryHandler<User, GetUserByIdQuery> queryHandler,
            [FromServices] IMapper mapper )
    {
        GetUserByIdQuery query = new GetUserByIdQuery
        {
            Id = id,
        };

        Result<User> result = await queryHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return NotFound( result.Error );
        }

        UserDetailDto dto = mapper.Map<UserDetailDto>( result.Value );
        return Ok( dto );
    }

    [HttpDelete( "{id}" )]
    [Authorize( Policy = nameof( PermissionName.UserDelete ) )]
    public async Task<ActionResult<string>> DeleteUser(
    [FromRoute] int id,
    [FromServices] ICommandHandlerWithResult<DeleteUserCommand, string> commandHandler )
    {
        DeleteUserCommand command = new DeleteUserCommand
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
