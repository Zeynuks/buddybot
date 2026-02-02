using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Commands.CreateMentor;
public class CreateMentorCommand
{
    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов." )]
    public string? FirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов." )]
    public string? LastName { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на фото не должна превышать {1} символов." )]
    public string? MentorPhotoUrl { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на MS Teams не должна превышать {1} символов." )]
    public string? MicrosoftTeamsUrl { get; set; }
    public int TeamId { get; set; }
    public bool IsTeamLeader { get; set; }
}
