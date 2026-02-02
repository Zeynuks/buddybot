namespace Contracts.NotificationDtos;
public class OnboardingFeedbackReminderRequest
{
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
}