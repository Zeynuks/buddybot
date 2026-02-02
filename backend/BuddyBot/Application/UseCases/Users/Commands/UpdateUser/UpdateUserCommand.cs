using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Commands.UpdateUser;
public class UpdateUserCommand
{
    public int Id { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов." )]
    public string? FirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов." )]
    public string? LastName { get; set; }
    public int? TeamId { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на MS Teams не должна превышать {1} символов." )]
    public string? MicrosoftTeamsUrl { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на фото не должна превышать {1} символов." )]
    public string? PhotoUrl { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на видео не должна превышать {1} символов." )]
    public string? VideoUrl { get; set; }
    public List<int>? HrIds { get; set; }
    public List<int>? MentorIds { get; set; }
    public DateTime? OnboardingAccessTimeUtc { get; set; }

    [StringLength( 100, ErrorMessage = "Ссылка на телеграм не должна превышать {1} символов." )]
    public string? TelegramContact { get; set; }

}
