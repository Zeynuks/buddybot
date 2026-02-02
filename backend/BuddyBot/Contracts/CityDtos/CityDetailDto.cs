using System.ComponentModel.DataAnnotations;

namespace Contracts.CityDtos;

public class CityDetailDto
{
    [Required]
    public int Id { get; init; }

    [MaxLength( 100 )]
    public string? Name { get; init; }

    [Required]
    public int CountryId { get; init; }

    [Required]
    public string? CountryName { get; init; }

    [Required]
    public int CandidateCount { get; init; }

}
