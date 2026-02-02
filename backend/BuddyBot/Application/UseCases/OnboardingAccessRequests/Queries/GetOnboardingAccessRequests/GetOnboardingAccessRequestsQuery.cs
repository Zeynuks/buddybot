using Domain.Enums;

namespace Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequests;
public class GetOnboardingAccessRequestsQuery
{
    public string? SearchTerm { get; set; }
    public RequestOutcome? RequestOutcome { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
