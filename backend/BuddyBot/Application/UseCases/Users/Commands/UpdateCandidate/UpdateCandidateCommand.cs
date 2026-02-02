using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Commands.UpdateCandidate;
public class UpdateCandidateCommand
{
    public int Id { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов." )]
    public string? FirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов." )]
    public string? LastName { get; set; }

    public int? TeamId { get; set; }

    public List<int>? HRIds { get; set; }
    public List<int>? MentorIds { get; set; }

    public string? CurrentStep { get; set; }
}
