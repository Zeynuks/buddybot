using Application.Interfaces;
using Contracts.NotificationDtos;

namespace Scheduler.Notifications;

public class NotificationHttpClient : INotificationPublisher
{
    private readonly HttpClient _httpClient;

    public NotificationHttpClient( HttpClient httpClient )
    {
        _httpClient = httpClient;
    }

    public async Task PublishOnboardingGrantedAsync( FeedbackReminderRequest request )
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            "/api/notifications/onboarding-granted",
            request
        );
        response.EnsureSuccessStatusCode();
    }
}