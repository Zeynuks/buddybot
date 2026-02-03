using Contracts.NotificationDtos;
using Microsoft.AspNetCore.Mvc;
using TelegramBot.Services;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("onboarding-granted")]
    public IActionResult OnboardingGranted([FromBody] FeedbackReminderRequest request)
    {
        _notificationService.ScheduleOnboardingStartReminders(
            request.TelegramId,
            request.FirstName,
            request.AccessTimeUtc
        );
        return Ok();
    }
}