using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Feedbacks.Commands.ConfirmFeedback;
public class ConfirmFeedbackCommandHandler(
    IFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    ILogger<ConfirmFeedbackCommand> logger )
    : CommandBaseHandlerWithResult<ConfirmFeedbackCommand, Feedback>( logger )
{
    protected override async Task<Result<Feedback>> HandleImplAsync( ConfirmFeedbackCommand command )
    {
        Feedback? feedback = await feedbackRepository.Get( command.FeedbackId );
        if ( feedback == null )
        {
            return Result<Feedback>.FromError( "Отзыв не найден." );
        }

        feedback.ConfirmByUser( command.Comment );

        await unitOfWork.CommitAsync();

        return Result<Feedback>.FromSuccess( feedback );
    }
}
