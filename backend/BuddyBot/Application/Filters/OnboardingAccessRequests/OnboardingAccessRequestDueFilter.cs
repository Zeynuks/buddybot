using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.OnboardingAccessRequests;
public class OnboardingAccessRequestDueFilter : IFilter<OnboardingAccessRequest>
{
    public DateTime Cutoff { get; set; }

    public IQueryable<OnboardingAccessRequest> Apply( IQueryable<OnboardingAccessRequest> query )
    {
        return query.Where( oar =>
            oar.Candidate != null &&
            oar.Candidate.OnboardingAccessTimeUtc.HasValue &&
            oar.Candidate.OnboardingAccessTimeUtc.Value <= Cutoff
        );
    }
}
