using Domain.Entities;
using Domain.Enums;
using Domain.Filters;

namespace Application.Filters.Feedbacks;
public class FeedbackStageTypeFilter : IFilter<Feedback>
{
    public ProcessKind? StageType { get; set; }

    public IQueryable<Feedback> Apply( IQueryable<Feedback> query )
    {
        if ( StageType.HasValue )
        {
            query = query.Where( f => f.ProcessKind == StageType.Value );
        }
        return query;
    }
}
