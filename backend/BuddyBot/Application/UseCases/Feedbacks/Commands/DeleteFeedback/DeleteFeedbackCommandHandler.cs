using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Feedbacks.Commands.DeleteFeedback;
public class DeleteFeedbackCommandHandler(
    IFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteFeedbackCommand> logger )
    : CommandBaseHandlerWithResult<DeleteFeedbackCommand, string>( logger )
{
    protected override async Task<Result<string>> HandleImplAsync( DeleteFeedbackCommand command )
    {
        if ( command.Id <= 0 )
        {
            return Result<string>.FromError( "ID отзыва должен быть положительным." );
        }

        Feedback? feedback = await feedbackRepository.Get( command.Id );
        if ( feedback == null )
        {
            return Result<string>.FromError( $"Отзыв с ID {command.Id} не найден." );
        }

        feedback.SoftDelete();
        await unitOfWork.CommitAsync();

        return Result<string>.FromSuccess( $"Отзыв с ID {command.Id} был успешно удалён." );
    }
}
