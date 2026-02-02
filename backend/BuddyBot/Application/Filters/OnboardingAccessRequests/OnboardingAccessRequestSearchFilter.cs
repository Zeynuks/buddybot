using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.OnboardingAccessRequests;
public class OnboardingAccessRequestSearchFilter : IFilter<OnboardingAccessRequest>
{
    public string? SearchTerm { get; set; }

    public IQueryable<OnboardingAccessRequest> Apply( IQueryable<OnboardingAccessRequest> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( oar =>
                oar.Candidate != null &&
                oar.Candidate.ContactInfo != null &&
                (
                    ( !string.IsNullOrEmpty( oar.Candidate.ContactInfo.FirstName ) &&
                     oar.Candidate.ContactInfo.FirstName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( !string.IsNullOrEmpty( oar.Candidate.ContactInfo.LastName ) &&
                     oar.Candidate.ContactInfo.LastName.ToLower().Contains( lowerSearchTerm ) ) ||
                    (
                     !string.IsNullOrEmpty( oar.Candidate.ContactInfo.FirstName ) &&
                     !string.IsNullOrEmpty( oar.Candidate.ContactInfo.LastName ) &&
                     ( oar.Candidate.ContactInfo.FirstName + " " + oar.Candidate.ContactInfo.LastName )
                         .ToLower().Contains( lowerSearchTerm )
                    )
                )
            );
        }
        return query;
    }
}
