using System.ComponentModel.DataAnnotations;

namespace Contracts.UserDtos;

public class RegisterUserDto
{
    [Required]
    [MaxLength( 100 )]
    public string? Login { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Password { get; init; }
}
