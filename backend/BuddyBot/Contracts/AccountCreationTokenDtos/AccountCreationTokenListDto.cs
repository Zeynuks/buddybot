using Contracts.UserDtos;
using System.ComponentModel.DataAnnotations;

namespace Contracts.AccountCreationTokenDtos;

public class AccountCreationTokenListDto
{
    [Required]
    public Guid TokenValue { get; init; }

    [Required]
    public string? UserFirstName { get; init; }

    [Required]
    public string? UserLastName { get; init; }

    [Required]
    public string? Status { get; init; }

    [Required]
    public DateTime IssuedAt { get; init; }
    public DateTime? ExpireDate { get; init; }
    public DateTime? ActivatedAt { get; init; }

    public string? UserRole { get; init; }

    public List<UserLookupDto>? HRs { get; init; }
}
