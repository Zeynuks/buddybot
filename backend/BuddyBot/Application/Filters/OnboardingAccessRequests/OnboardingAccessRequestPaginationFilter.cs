using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.OnboardingAccessRequests;
public class OnboardingAccessRequestPaginationFilter : IFilter<OnboardingAccessRequest>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public IQueryable<OnboardingAccessRequest> Apply( IQueryable<OnboardingAccessRequest> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
