using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.AccountCreationTokens.Commands.UpdateToken;
public class UpdateTokenCommand
{
    public Guid TokenValue { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов." )]
    public string? FirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов." )]
    public string? LastName { get; set; }

    public int? TeamId { get; set; }
    public DateTime? ExpireDate { get; set; }
    public List<int>? MentorIds { get; set; }
    public List<int>? HRIds { get; set; }

    [StringLength( 100, ErrorMessage = "Ссылка на телеграм не должна превышать {1} символов." )]
    public string? TelegramContact { get; set; }
}
