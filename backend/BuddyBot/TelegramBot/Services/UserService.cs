using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.Users.Commands.ActivateUserFromToken;
using Application.UseCases.Users.Queries.GetUserByTelegramId;
using Domain.Entities;

namespace TelegramBot.Services;
public class UserService(
    ICommandHandlerWithResult<ActivateUserFromTokenCommand, User> activateUserHandler,
    IQueryHandler<User, GetUserByTelegramIdQuery> getUserByTelegramIdHandler)
{
    public async Task<Result<User>> ActivateUser( Guid token, long telegramId )
    {
        ActivateUserFromTokenCommand command = new ActivateUserFromTokenCommand
        {
            TokenValue = token,
            TelegramId = telegramId
        };

        Result<User> result = await activateUserHandler.HandleAsync( command );

        return result;
    }

    public async Task<User?> GetUserByTelegramId( long telegramId )
    {
        GetUserByTelegramIdQuery query = new GetUserByTelegramIdQuery
        {
            TelegramId = telegramId
        };

        Result<User> result = await getUserByTelegramIdHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return null;
        }

        return result.Value;
    }
}