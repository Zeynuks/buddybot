using Application.CQRSInterfaces;
using Application.Filters.Departments;
using Application.Filters.Users;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.Queries.GetUsers;
public class GetUsersQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUsersQuery> logger )
    : QueryBaseHandler<PagedResult<User>, GetUsersQuery>( logger )
{
    protected override async Task<Result<PagedResult<User>>> HandleImplAsync( GetUsersQuery query )
    {
        List<IFilter<User>> filters = new()
        {
            new UserSearchFilter { SearchTerm = query.SearchTerm },
            new UserRoleFilter { Roles = query.Roles },
            new UserDepartmentFilter { DepartmentId = query.DepartmentId },
            new UserTeamFilter { TeamId = query.TeamId },
            new UserProcessKindFilter { ProcessKind = query.ProcessKind },
            new UserNotDeletedFilter()
        };

        int totalCount = await userRepository.CountFilteredUsers( filters );
        filters.Add( new UserPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<User> users = await userRepository.GetFilteredUsers( filters );

        return Result<PagedResult<User>>.FromSuccess( new PagedResult<User>
        {
            Items = users,
            TotalCount = totalCount
        } );
    }
}