using System.ComponentModel.DataAnnotations;

namespace Contracts.UserDtos;

public class TokenDto
{
    [Required]
    [MaxLength( 200 )]
    public string? AccessToken { get; init; }

    [Required]
    [MaxLength( 200 )]
    public string? RefreshToken { get; init; }
}