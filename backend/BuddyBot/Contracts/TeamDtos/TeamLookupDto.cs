using System.ComponentModel.DataAnnotations;

namespace Contracts.TeamDtos;

public class TeamLookupDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }
}
