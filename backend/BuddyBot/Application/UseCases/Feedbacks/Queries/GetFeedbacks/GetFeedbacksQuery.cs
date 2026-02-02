using Domain.Enums;

namespace Application.UseCases.Feedbacks.Queries.GetFeedbacks;
public class GetFeedbacksQuery
{
    public string? SearchTerm { get; set; }
    public ProcessKind? StageType { get; set; }
    public int? Rating { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
