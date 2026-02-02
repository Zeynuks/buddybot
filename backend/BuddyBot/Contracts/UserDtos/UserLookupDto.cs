using System.ComponentModel.DataAnnotations;

namespace Contracts.UserDtos;

public class UserLookupDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string? FirstName { get; init; }

    [Required]
    public string? LastName { get; init; }

    public List<string>? Roles { get; init; }
}
