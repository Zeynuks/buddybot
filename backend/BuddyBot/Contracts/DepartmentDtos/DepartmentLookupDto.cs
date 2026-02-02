using System.ComponentModel.DataAnnotations;

namespace Contracts.DepartmentDtos;

public class DepartmentLookupDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    [MaxLength( 100 )]
    public string? Name { get; init; }
}
