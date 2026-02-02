using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Queries.GetUserById;
public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserByIdQuery> logger )
    : QueryBaseHandler<User, GetUserByIdQuery>( logger )
{
    protected override async Task<Result<User>> HandleImplAsync( GetUserByIdQuery query )
    {
        User? user = await userRepository.Get( query.Id );
        if ( user == null )
        {
            return Result<User>.FromError( $"Пользователь с ID {query.Id} не найден." );
        }

        return Result<User>.FromSuccess( user );
    }
}
