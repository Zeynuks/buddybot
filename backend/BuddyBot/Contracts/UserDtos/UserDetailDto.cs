using System.ComponentModel.DataAnnotations;
using Contracts.TeamDtos;

namespace Contracts.UserDtos;
public class UserDetailDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string? FirstName { get; init; }

    [Required]
    public string? LastName { get; init; }
    public string? Login { get; init; }

    public List<string>? Roles { get; init; }

    public long? TelegramId { get; init; }

    [Required]
    public DateTime CreatedAt { get; init; }
    public string? TelegramContact { get; init; }

    public string? PhotoUrl { get; init; }
    public string? VideoUrl { get; init; }
    public string? MicrosoftTeamsUrl { get; init; }
    public int? CityId { get; init; }
    public string? CityName { get; init; }

    public List<UserLookupDto>? HRs { get; init; }
    public List<UserLookupDto>? Mentors { get; init; }
    public TeamDetailDto? Team { get; init; }
    public bool IsTeamLeader { get; init; }
    public string? ActiveProcessKind { get; init; }
    public bool IsActivated { get; init; }
    public bool IsOnboardingAccessGranted { get; init; }
}
