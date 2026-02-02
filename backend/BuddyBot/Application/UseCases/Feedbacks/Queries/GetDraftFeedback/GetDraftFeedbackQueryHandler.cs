using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Feedbacks.Queries.GetDraftFeedback;
public class GetDraftFeedbackQueryHandler(
    IFeedbackRepository feedbackRepository,
    ILogger<GetDraftFeedbackQuery> logger )
    : QueryBaseHandler<Feedback, GetDraftFeedbackQuery>( logger )
{
    protected override async Task<Result<Feedback>> HandleImplAsync( GetDraftFeedbackQuery query )
    {
        Feedback? feedback = await feedbackRepository.GetDraftFeedback( query.CandidateId, query.ProcessKind );

        if ( feedback == null )
        {
            return Result<Feedback>.FromError( "Нет черновика отзыва для данного этапа." );
        }

        return Result<Feedback>.FromSuccess( feedback );
    }
}
