using Application.CQRSInterfaces;
using Application.Filters.AccountCreationTokens;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokens;
public class GetAccountCreationTokensQueryHandler(
    IAccountCreationTokenRepository accountCreationTokenRepository,
    ILogger<GetAccountCreationTokensQuery> logger )
    : QueryBaseHandler<PagedResult<AccountCreationToken>, GetAccountCreationTokensQuery>( logger )
{
    protected override async Task<Result<PagedResult<AccountCreationToken>>> HandleImplAsync( GetAccountCreationTokensQuery query )
    {
        List<IFilter<AccountCreationToken>> filters = new List<IFilter<AccountCreationToken>>
        {
            new AccountCreationTokenSearchFilter { SearchTerm = query.SearchTerm },
            new AccountCreationTokenStatusFilter { Status = query.Status },
            new AccountCreationTokenRoleFilter { Roles = query.Roles }
        };

        int totalCount = await accountCreationTokenRepository.CountFilteredTokens( filters );
        filters.Add( new AccountCreationTokenPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<AccountCreationToken> tokens = await accountCreationTokenRepository.GetFilteredTokens( filters );

        return Result<PagedResult<AccountCreationToken>>.FromSuccess( new PagedResult<AccountCreationToken>
        {
            Items = tokens,
            TotalCount = totalCount
        } );
    }
}
