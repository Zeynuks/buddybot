using Hangfire;
using Scheduler.Schedulers;

namespace Scheduler.Jobs;

public static class RecurringJobsRegistration
{
    public static void Register( IServiceProvider services )
    {
        RecurringJob.AddOrUpdate<OnboardingAccessScheduler>(
            "GrantOnboardingAccess",
            s => s.ProcessDueRequests(),
            Cron.Minutely
        );

        RecurringJob.AddOrUpdate<AccountCreationTokenExpirationScheduler>(
            "MarkExpiredTokens",
            s => s.ProcessExpiredTokens(),
            Cron.Hourly
        );
    }
}