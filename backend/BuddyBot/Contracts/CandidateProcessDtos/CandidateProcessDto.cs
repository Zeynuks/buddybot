using Contracts.UserDtos;

namespace Contracts.CandidateProcessDtos;

public class CandidateProcessDto
{
    public int CandidateId { get; init; }
    public int ProcessKind { get; init; }
    public string? CurrentStep { get; init; }
    public UserLookupDto? Candidate { get; init; }
}