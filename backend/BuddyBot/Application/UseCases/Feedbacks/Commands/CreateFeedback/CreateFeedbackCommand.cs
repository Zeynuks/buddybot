using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackCommand
{
    public int CandidateId { get; set; }

    public ProcessKind ProcessKind { get; set; }

    [Range( 0, 5, ErrorMessage = "Оценка должна быть от 0 до 5." )]
    public int Rating { get; set; }

    [StringLength( 4096, ErrorMessage = "Комментарий не должен превышать {1} символов." )]
    public string? Comment { get; set; }
}
