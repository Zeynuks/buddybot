using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Feedbacks.Commands.ConfirmFeedback;
public class ConfirmFeedbackCommand
{
    public int FeedbackId { get; set; }

    [StringLength( 4096, ErrorMessage = "Комментарий не должен превышать {1} символов." )]
    public string? Comment { get; set; }
}
