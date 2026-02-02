using System.ComponentModel.DataAnnotations;

namespace Contracts.TeamDtos;
public class TeamDetailDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }

    [Required]
    public int DepartmentId { get; init; }

    [MaxLength( 100 )]
    public string? DepartmentName { get; init; }
    public int? LeaderId { get; init; }
}
