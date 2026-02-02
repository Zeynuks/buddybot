using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.OnboardingAccessRequests;
public class OnboardingAccessRequestStatusFilter : IFilter<OnboardingAccessRequest>
{
    public RequestOutcome? RequestOutcome { get; set; }

    public IQueryable<OnboardingAccessRequest> Apply( IQueryable<OnboardingAccessRequest> query )
    {
        if ( RequestOutcome.HasValue )
        {
            query = query.Where( oar => oar.RequestOutcome == RequestOutcome.Value );
        }
        return query;
    }
}
