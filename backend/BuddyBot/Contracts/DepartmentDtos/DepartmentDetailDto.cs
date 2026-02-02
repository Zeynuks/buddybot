using Contracts.TeamDtos;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DepartmentDtos;

public class DepartmentDetailDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }

    [MaxLength( 50 )]
    public string? HeadFirstName { get; init; }

    [MaxLength( 50 )]
    public string? HeadLastName { get; init; }

    [MaxLength( 255 )]
    public string? HeadVideoUrl { get; init; }

    [MaxLength( 255 )]
    public string? HeadMicrosoftTeamsUrl { get; init; }

    public List<TeamListDto> Teams { get; init; } = new();
}
