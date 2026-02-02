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

        backgroundJobClient.Schedule<FeedbackNotifier>(
            notifier => notifier.NotifyPreboarding( telegramId, firstName ),
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

        backgroundJobClient.Schedule<FeedbackNotifier>(
            notifier => notifier.NotifyOnboarding( telegramId, firstName ),
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
            backgroundJobClient.Schedule<OnboardingNotifier>(
                notifier => notifier.NotifyStartReminder( telegramId, firstName ),
                firstDelay
            );
        }

        DateTime secondReminderUtc = GetMoscowMorning( onboardingAccessTimeUtc, OnboardingSecondReminderDayOffset );
        TimeSpan secondDelay = secondReminderUtc - DateTime.UtcNow;
        if ( secondDelay > TimeSpan.Zero )
        {
            backgroundJobClient.Schedule<OnboardingNotifier>(
                notifier => notifier.NotifyStartReminder( telegramId, firstName ),
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