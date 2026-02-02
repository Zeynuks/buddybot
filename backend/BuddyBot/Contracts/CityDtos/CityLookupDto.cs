using System.ComponentModel.DataAnnotations;

namespace Contracts.CityDtos;

public class CityLookupDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }
}
