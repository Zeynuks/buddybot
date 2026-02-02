using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Feedbacks;
public class FeedbackRatingFilter : IFilter<Feedback>
{
    public int? Rating { get; set; }

    public IQueryable<Feedback> Apply( IQueryable<Feedback> query )
    {
        if ( Rating.HasValue )
        {
            query = query.Where( f => f.Rating == Rating.Value );
        }
        return query;
    }
}
