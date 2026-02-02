using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.AccountCreationTokens.Commands.CreateHRInvite;
public class CreateHRInviteCommand
{
    [StringLength( 100, MinimumLength = 3, ErrorMessage = "Логин должен быть от {2} до {1} символов." )]
    public string? Login { get; set; }

    [StringLength( 100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов." )]
    public string? Password { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов." )]
    public string? FirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов." )]
    public string? LastName { get; set; }

    [StringLength( 100, ErrorMessage = "Ссылка на телеграм не должна превышать {1} символов." )]
    public string? TelegramContact { get; set; }

    [StringLength( 255, ErrorMessage = "Ссылка на MS Teams не должна превышать {1} символов." )]
    public string? MicrosoftTeamsUrl { get; set; }

    public int TeamId { get; set; }
    public int ExpirationDays { get; set; }
    public int CreatorId { get; set; }
}
