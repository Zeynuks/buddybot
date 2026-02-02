using System.ComponentModel.DataAnnotations;

namespace Contracts.FeedbackDtos;

public class FeedbackListDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string? FirstName { get; init; }

    [Required]
    public string? LastName { get; init; }

    [Required]
    public string? ProcessKind { get; init; }

    [Required]
    public int Rating { get; init; }

    [MaxLength( 4096 )]
    public string? Comment { get; init; }

    public string? DepartmentName { get; init; }
    public string? TeamName { get; init; }

    public List<string> HRNames { get; init; } = new List<string>();
    public List<string> MentorNames { get; init; } = new List<string>();

    [Required]
    public DateTime CreatedAt { get; init; }
}
