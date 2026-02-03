using Contracts.NotificationDtos;
using Hangfire;
using TelegramBot.Notifiers;

namespace TelegramBot.Services;
public class NotificationService( IBackgroundJobClient backgroundJobClient )
{
    private const int PreboardingFeedbackDelayDays = 3;
    private const int OnboardingFeedbackDelayDays = 21;

    private const int OnboardingFirstReminderDayOffset = 1;
    private const int OnboardingSecondReminderDayOffset = 4;
    private const int MoscowMorningHour = 9;
    private const int MoscowUtcOffset = 3;

    public bool SchedulePreboardingFeedback( long telegramId, string? firstName )
    {
        if ( telegramId == 0 || string.IsNullOrWhiteSpace( firstName ) )
        {
            return false;
        }

        FeedbackReminderRequest request = new()
        {
            TelegramId = telegramId,
            FirstName = firstName,
            AccessTimeUtc = DateTime.UtcNow
        };
        
        backgroundJobClient.Schedule<FeedbackNotifier>(
            notifier => notifier.NotifyPreboarding( request ),
            TimeSpan.FromDays( PreboardingFeedbackDelayDays )
        );

        return true;
    }

    public bool ScheduleOnboardingFeedback( long telegramId, string? firstName )
    {
        if ( telegramId == 0 || string.IsNullOrWhiteSpace( firstName ) )
        {
            return false;
        }
        
        FeedbackReminderRequest request = new()
        {
            TelegramId = telegramId,
            FirstName = firstName,
            AccessTimeUtc = DateTime.UtcNow
        };

        backgroundJobClient.Schedule<FeedbackNotifier>(
            notifier => notifier.NotifyOnboarding( request ),
            TimeSpan.FromDays( OnboardingFeedbackDelayDays )
        );

        return true;
    }

    public bool ScheduleOnboardingStartReminders( long telegramId, string? firstName, DateTime onboardingAccessTimeUtc )
    {
        if ( telegramId == 0 || string.IsNullOrWhiteSpace( firstName ) )
        {
            return false;
        }

        DateTime firstReminderUtc = GetMoscowMorning( onboardingAccessTimeUtc, OnboardingFirstReminderDayOffset );
        TimeSpan firstDelay = firstReminderUtc - DateTime.UtcNow;
        if ( firstDelay > TimeSpan.Zero )
        {
            FeedbackReminderRequest request = new()
            {
                TelegramId = telegramId,
                FirstName = firstName,
                AccessTimeUtc = DateTime.UtcNow
            };
            
            backgroundJobClient.Schedule<OnboardingNotifier>(
                notifier => notifier.NotifyStartReminder( request ),
                firstDelay
            );
        }

        DateTime secondReminderUtc = GetMoscowMorning( onboardingAccessTimeUtc, OnboardingSecondReminderDayOffset );
        TimeSpan secondDelay = secondReminderUtc - DateTime.UtcNow;
        if ( secondDelay > TimeSpan.Zero )
        {
            FeedbackReminderRequest request = new()
            {
                TelegramId = telegramId,
                FirstName = firstName,
                AccessTimeUtc = DateTime.UtcNow
            };
            
            backgroundJobClient.Schedule<OnboardingNotifier>(
                notifier => notifier.NotifyStartReminder( request ),
                secondDelay
            );
        }

        return true;
    }

    private static DateTime GetMoscowMorning( DateTime baseUtc, int daysAfter )
    {
        DateTime mskDate = baseUtc.AddHours( MoscowUtcOffset ).Date;
        DateTime mskTarget = mskDate.AddDays( daysAfter ).AddHours( MoscowMorningHour );
        DateTime utcTarget = mskTarget.AddHours( -MoscowUtcOffset );
        return utcTarget;
    }
}