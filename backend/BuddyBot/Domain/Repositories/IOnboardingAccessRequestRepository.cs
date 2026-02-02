using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface IOnboardingAccessRequestRepository : IBaseRepository<OnboardingAccessRequest>
{
    Task<int> CountFilteredOnboardingAccessRequests( IEnumerable<IFilter<OnboardingAccessRequest>> filters );
    Task<List<OnboardingAccessRequest>> GetFilteredOnboardingAccessRequests( IEnumerable<IFilter<OnboardingAccessRequest>> filters );
    Task<List<User>> GetDueCandidates( DateTime utcNow );
}
