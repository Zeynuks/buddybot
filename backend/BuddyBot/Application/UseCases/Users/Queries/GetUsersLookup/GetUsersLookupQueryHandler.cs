using Application.CQRSInterfaces;
using Application.Filters.Users;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Queries.GetUsersLookup;
public class GetUsersLookupQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUsersLookupQuery> logger )
    : QueryBaseHandler<List<User>, GetUsersLookupQuery>( logger )
{
    protected async override Task<Result<List<User>>> HandleImplAsync( GetUsersLookupQuery query )
    {
        List<IFilter<User>> filters = new()
        {
            new UserRoleFilter { Roles = query.Roles },
            new UserTeamFilter { TeamId = query.TeamId },
            new UserVerifiedHRFilter { OnlyVerifiedHR = true },
            new UserNotDeletedFilter()

        };

        List<User> users = await userRepository.GetFilteredUsers( filters );
        return Result<List<User>>.FromSuccess( users );
    }
}
