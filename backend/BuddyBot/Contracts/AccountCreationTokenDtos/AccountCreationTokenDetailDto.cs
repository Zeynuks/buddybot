using System.ComponentModel.DataAnnotations;
using Contracts.DepartmentDtos;
using Contracts.TeamDtos;
using Contracts.UserDtos;

namespace Contracts.AccountCreationTokenDtos;

public class AccountCreationTokenDetailDto
{
    [Required]
    public Guid TokenValue { get; init; }

    [Required]
    public string? UserFirstName { get; init; }

    [Required]
    public string? UserLastName { get; init; }

    [Required]
    public string? InviteLink { get; init; }

    [Required]
    public string? QrCodeBase64 { get; init; }

    [Required]
    public string? Status { get; init; }

    [Required]
    public DateTime IssuedAt { get; init; }

    [Required]
    public DateTime? ExpireDate { get; init; }
    public DateTime? ActivatedAt { get; init; }

    [Required]
    public UserLookupDto? Creator { get; init; }
    public string? UserRole { get; init; }

    [Required]
    public UserDetailDto? User { get; init; }
    public DepartmentLookupDto? Department { get; init; }
    public TeamLookupDto? Team { get; init; }
    public List<UserLookupDto>? HRs { get; init; }
    public List<UserLookupDto>? Mentors { get; init; }
}
