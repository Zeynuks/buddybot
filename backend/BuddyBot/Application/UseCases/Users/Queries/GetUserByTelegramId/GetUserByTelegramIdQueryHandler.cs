using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Queries.GetUserByTelegramId;
public class GetUserByTelegramIdQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserByTelegramIdQuery> logger )
    : QueryBaseHandler<User, GetUserByTelegramIdQuery>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( GetUserByTelegramIdQuery query )
    {
        User? user = await userRepository.GetByTelegramId( query.TelegramId );
        if ( user == null )
        {
            return Result<User>.FromError( $"Пользователь с TelegramId {query.TelegramId} не найден." );
        }

        return Result<User>.FromSuccess( user );
    }
}
