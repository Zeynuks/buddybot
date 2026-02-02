namespace Contracts.NotificationDtos;
public class PreboardingFeedbackReminderRequest
{
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
}