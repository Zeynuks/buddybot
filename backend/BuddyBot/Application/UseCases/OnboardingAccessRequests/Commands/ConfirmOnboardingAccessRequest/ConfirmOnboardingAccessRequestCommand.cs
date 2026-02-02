namespace Application.UseCases.OnboardingAccessRequests.Commands.ConfirmOnboardingAccessRequest;
public class ConfirmOnboardingAccessRequestCommand
{
    public int CandidateId { get; set; }
    public DateTime OnboardingAccessTimeUtc { get; set; }
    public int? TeamId { get; set; }
    public List<int>? MentorIds { get; set; }
}
