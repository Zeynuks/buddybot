using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Feedbacks;
public class FeedbackSearchFilter : IFilter<Feedback>
{
    public string? SearchTerm { get; set; }

    public IQueryable<Feedback> Apply( IQueryable<Feedback> query )
    {
        if ( !string.IsNullOrEmpty( SearchTerm ) )
        {
            string lowerSearchTerm = SearchTerm.ToLower();
            query = query.Where( f =>
                f.Candidate != null &&
                f.Candidate.ContactInfo != null && (
                    ( !string.IsNullOrEmpty( f.Candidate.ContactInfo.FirstName ) && f.Candidate.ContactInfo.FirstName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( !string.IsNullOrEmpty( f.Candidate.ContactInfo.LastName ) && f.Candidate.ContactInfo.LastName.ToLower().Contains( lowerSearchTerm ) ) ||
                    ( !string.IsNullOrEmpty( f.Candidate.ContactInfo.FirstName ) && !string.IsNullOrEmpty( f.Candidate.ContactInfo.LastName ) &&
                        ( f.Candidate.ContactInfo.FirstName + " " + f.Candidate.ContactInfo.LastName ).ToLower().Contains( lowerSearchTerm ) )
                )
            );
        }
        return query;
    }
}
