using Application.CQRSInterfaces;
using Application.Filters.Departments;
using Application.Filters.OnboardingAccessRequests;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequests;
public class GetOnboardingAccessRequestsQueryHandler( 
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    ILogger<GetOnboardingAccessRequestsQuery> logger )
    : QueryBaseHandler<PagedResult<OnboardingAccessRequest>, GetOnboardingAccessRequestsQuery>(logger)
{
    protected override async Task<Result<PagedResult<OnboardingAccessRequest>>> HandleImplAsync( GetOnboardingAccessRequestsQuery query )
    {
        List<IFilter<OnboardingAccessRequest>> filters = new List<IFilter<OnboardingAccessRequest>>
        {
            new OnboardingAccessRequestSearchFilter { SearchTerm = query.SearchTerm },
            new OnboardingAccessRequestStatusFilter { RequestOutcome = query.RequestOutcome },
        };

        int totalCount = await onboardingAccessRequestRepository.CountFilteredOnboardingAccessRequests( filters );
        filters.Add( new OnboardingAccessRequestPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<OnboardingAccessRequest> onboardingAccessRequests = await onboardingAccessRequestRepository.GetFilteredOnboardingAccessRequests( filters );

        return Result<PagedResult<OnboardingAccessRequest>>.FromSuccess( new PagedResult<OnboardingAccessRequest>
        {
            Items = onboardingAccessRequests,
            TotalCount = totalCount
        } );
    }
}