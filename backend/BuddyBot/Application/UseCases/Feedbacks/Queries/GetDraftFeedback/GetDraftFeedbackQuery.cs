using Domain.Enums;

namespace Application.UseCases.Feedbacks.Queries.GetDraftFeedback;
public class GetDraftFeedbackQuery
{
    public int CandidateId { get; set; }
    public ProcessKind ProcessKind { get; set; }
}
