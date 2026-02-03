namespace Contracts.NotificationDtos;
public class FeedbackReminderRequest
{
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
    public DateTime AccessTimeUtc { get; set; }
}