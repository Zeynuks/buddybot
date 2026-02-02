using Domain.Enums;

namespace Application.UseCases.OnboardingAccessRequests.Commands.CreateOnboardingAccessRequest;
public class CreateOnboardingAccessRequestCommand
{
    public int CandidateId { get; set; }
    public RequestOutcome? Outcome { get; set; }
}
