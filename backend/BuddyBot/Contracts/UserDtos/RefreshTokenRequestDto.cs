using System.ComponentModel.DataAnnotations;

namespace Contracts.UserDtos;

public class RefreshTokenRequestDto
{
    [Required]
    [MaxLength( 200 )]
    public string? RefreshToken { get; init; }
}
