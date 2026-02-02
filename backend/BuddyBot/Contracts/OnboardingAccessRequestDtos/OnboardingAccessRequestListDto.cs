using System.ComponentModel.DataAnnotations;
using Contracts.DepartmentDtos;
using Contracts.TeamDtos;
using Contracts.UserDtos;

namespace Contracts.OnboardingAccessRequestDtos;

public class OnboardingAccessRequestListDto
{
    [Required]
    public string? RequestOutcome { get; init; }

    [Required]
    public int CandidateId { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? FirstName { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? LastName { get; init; }

    public DepartmentLookupDto? Department { get; init; }
    public TeamLookupDto? Team { get; init; }
    public List<UserLookupDto>? HRs { get; init; }
    public List<UserLookupDto>? Mentors { get; init; }

    [Required]
    public DateTime CreatedAt { get; init; }

    public DateTime? OnboardingAccessTime { get; init; }
}