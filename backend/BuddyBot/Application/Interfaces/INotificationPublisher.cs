using Contracts.NotificationDtos;

namespace Application.Interfaces;

public interface INotificationPublisher
{
    Task PublishOnboardingGrantedAsync(FeedbackReminderRequest request);
}