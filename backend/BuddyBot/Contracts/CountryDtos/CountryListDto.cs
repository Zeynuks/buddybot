using System.ComponentModel.DataAnnotations;

namespace Contracts.CountryDtos;

public class CountryListDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }

    [Required]
    public int CitiesCount { get; init; }
}
