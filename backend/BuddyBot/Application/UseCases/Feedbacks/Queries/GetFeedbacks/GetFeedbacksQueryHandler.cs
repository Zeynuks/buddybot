using Application.CQRSInterfaces;
using Application.Filters.Departments;
using Application.Filters.Feedbacks;
using Application.Results;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Feedbacks.Queries.GetFeedbacks;
public class GetFeedbacksQueryHandler( 
    IFeedbackRepository feedbackRepository,
    ILogger<GetFeedbacksQuery> logger )
    : QueryBaseHandler<PagedResult<Feedback>, GetFeedbacksQuery>(logger)

{
    protected override async Task<Result<PagedResult<Feedback>>> HandleImplAsync( GetFeedbacksQuery query )
    {
        List<IFilter<Feedback>> filters = new List<IFilter<Feedback>>
        {
            new FeedbackSearchFilter { SearchTerm = query.SearchTerm },
            new FeedbackStageTypeFilter { StageType = query.StageType },
            new FeedbackRatingFilter { Rating = query.Rating },
        };

        int totalCount = await feedbackRepository.CountFilteredFeedbacks( filters );
        filters.Add( new FeedbackPaginationFilter { PageNumber = query.PageNumber, PageSize = query.PageSize } );

        List<Feedback> feedbacks = await feedbackRepository.GetFilteredFeedbacks( filters );

        return Result<PagedResult<Feedback>>.FromSuccess( new PagedResult<Feedback>
        {
            Items = feedbacks,
            TotalCount = totalCount
        } );
    }
}
