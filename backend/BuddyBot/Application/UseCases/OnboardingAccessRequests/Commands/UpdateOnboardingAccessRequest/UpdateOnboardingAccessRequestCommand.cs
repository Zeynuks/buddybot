namespace Application.UseCases.OnboardingAccessRequests.Commands.UpdateOnboardingAccessRequest;
public class UpdateOnboardingAccessRequestCommand
{
    public int CandidateId { get; set; }
    public DateTime OnboardingAccessTimeUtc { get; set; }
    public int? TeamId { get; set; }
    public List<int>? MentorIds { get; set; }
}
