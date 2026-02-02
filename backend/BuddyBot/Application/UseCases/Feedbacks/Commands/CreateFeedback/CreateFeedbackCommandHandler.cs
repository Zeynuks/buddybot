using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Feedbacks.Commands.CreateFeedback;
public class CreateFeedbackCommandHandler(
    IFeedbackRepository repository,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    ILogger<CreateFeedbackCommand> logger )
    : CommandBaseHandlerWithResult<CreateFeedbackCommand, Feedback>(logger)
{
    protected override async Task<Result<Feedback>> HandleImplAsync( CreateFeedbackCommand command )
    {
        if ( command.CandidateId <= 0 )
        {
            return Result<Feedback>.FromError( "ID кандидата должен быть положительным." );
        }
        if ( !Enum.IsDefined( typeof( ProcessKind ), command.ProcessKind ) )
        {
            return Result<Feedback>.FromError( "Некорректный процесс фидбэка." );
        }
        if ( command.Rating < 1 || command.Rating > 5 )
        {
            return Result<Feedback>.FromError( "Оценка должна быть в диапазоне от 1 до 5." );
        }
        
        User? candidate = await userRepository.Get( command.CandidateId );
        if ( candidate == null )
        {
            return Result<Feedback>.FromError( $"Кандидат с ID {command.CandidateId} не найден." );
        }

        Feedback? feedback = new Feedback( 
            command.CandidateId,
            command.ProcessKind,
            command.Rating,
            command.Comment ?? string.Empty
        );

        repository.Add( feedback );
        await unitOfWork.CommitAsync();

        return Result<Feedback>.FromSuccess( feedback );
    }
}
