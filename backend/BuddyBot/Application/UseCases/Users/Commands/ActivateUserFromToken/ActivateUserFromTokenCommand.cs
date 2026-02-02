namespace Application.UseCases.Users.Commands.ActivateUserFromToken;
public class ActivateUserFromTokenCommand
{
    public Guid TokenValue { get; set; }
    public long TelegramId { get; set; }
}
