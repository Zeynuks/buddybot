using Domain.Entities;
using Domain.Filters;

namespace Application.Filters.Feedbacks;
public class FeedbackPaginationFilter : IFilter<Feedback>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IQueryable<Feedback> Apply( IQueryable<Feedback> query )
    {
        int skip = ( PageNumber - 1 ) * PageSize;
        return query.Skip( skip ).Take( PageSize );
    }
}
