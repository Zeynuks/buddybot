using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.Feedbacks.Commands.ConfirmFeedback;
using Application.UseCases.Feedbacks.Commands.CreateFeedback;
using Application.UseCases.Feedbacks.Queries.GetDraftFeedback;
using Domain.Entities;
using Domain.Enums;

namespace TelegramBot.Services;
public class FeedbackService(
    ICommandHandlerWithResult<CreateFeedbackCommand, Feedback> createFeedbackHandler,
    ICommandHandlerWithResult<ConfirmFeedbackCommand, Feedback> confirmFeedbackHandler,
    IQueryHandler<Feedback, GetDraftFeedbackQuery> getDraftFeedbackHandler
)
{
    public async Task<Result<Feedback>> CreateFeedback( int candidateId, ProcessKind processKind, int rating )
    {
        CreateFeedbackCommand command = new CreateFeedbackCommand
        {
            CandidateId = candidateId,
            ProcessKind = processKind,
            Rating = rating,
        };

        return await createFeedbackHandler.HandleAsync( command );
    }

    public async Task<Result<Feedback>> ConfirmFeedback( int feedbackId, string? comment )
    {
        ConfirmFeedbackCommand command = new ConfirmFeedbackCommand
        {
            FeedbackId = feedbackId,
            Comment = comment
        };

        return await confirmFeedbackHandler.HandleAsync( command );
    }

    public async Task<Feedback> GetDraftFeedback( int candidateId, ProcessKind processKind )
    {
        GetDraftFeedbackQuery query = new GetDraftFeedbackQuery
        {
            CandidateId = candidateId,
            ProcessKind = processKind
        };

        Result<Feedback> result = await getDraftFeedbackHandler.HandleAsync( query );
        return result.Value!;
    }
}
