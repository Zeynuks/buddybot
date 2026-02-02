using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.CandidateProcesses.Commands.TransferToOnboarding;
using Application.UseCases.OnboardingAccessRequests.Queries.GetDueCandidates;
using Domain.Entities;
using TelegramBot.Notifiers;
using TelegramBot.Services;

namespace Scheduler.Schedulers;
public class OnboardingAccessScheduler(
   
    IQueryHandler<List<User>, GetDueCandidatesQuery> getDueCandidatesQueryHandler,
    ICommandHandlerWithResult<TransferToOnboardingCommand, CandidateProcess> transferToOnboardingHandler,
    OnboardingNotifier notifier,
    NotificationService notificationService )
{
    public async Task ProcessDueRequests()
    {
        var utcNow = DateTime.UtcNow;
        GetDueCandidatesQuery query = new GetDueCandidatesQuery
        {
            UtcNow = utcNow,
        };

        Result<List<User>> usersResult = await getDueCandidatesQueryHandler.HandleAsync( query );

        if ( !usersResult.IsSuccess || usersResult.Value == null || usersResult.Value.Count == 0 )
        {
            return;
        }

        foreach ( User user in usersResult.Value )
        {
            TransferToOnboardingCommand command = new TransferToOnboardingCommand
            {
                CandidateId = user.Id
            };

            Result<CandidateProcess> transferResult = await transferToOnboardingHandler.HandleAsync( command );

            if ( transferResult.IsSuccess )
            {
                long? telegramId = user.ContactInfo?.TelegramId;
                string? firstName = user.ContactInfo?.FirstName;
                DateTime? onboardingAccessTimeUtc = user.OnboardingAccessTimeUtc;

                if ( telegramId != null && !string.IsNullOrWhiteSpace( firstName ) )
                {
                    await notifier.NotifyGranted( telegramId.Value, firstName );

                    notificationService.ScheduleOnboardingStartReminders( telegramId.Value, firstName, onboardingAccessTimeUtc.Value );
                }
            }
        }
    }
}