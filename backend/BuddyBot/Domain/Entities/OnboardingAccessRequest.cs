using Domain.Enums;

namespace Domain.Entities;
public class OnboardingAccessRequest
{
    public int CandidateId { get; init; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public RequestOutcome RequestOutcome { get; set; }
    public bool IsDeleted { get; private set; }

    public User? Candidate { get; init; }

    public OnboardingAccessRequest() { }

    public OnboardingAccessRequest( int candidateId )
    {
        CandidateId = candidateId;
        RequestOutcome = RequestOutcome.Pending;
    }

    public OnboardingAccessRequest( int candidateId, RequestOutcome outcome )
    : this( candidateId )
    {
        RequestOutcome = outcome;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}