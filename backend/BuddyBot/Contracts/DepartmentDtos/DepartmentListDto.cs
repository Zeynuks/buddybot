using System.ComponentModel.DataAnnotations;

namespace Contracts.DepartmentDtos;

public class DepartmentListDto
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

    [Required]
    public bool IsVideoGreetingUploaded { get; init; }

    [Required]
    public int TeamCount { get; init; }
}
