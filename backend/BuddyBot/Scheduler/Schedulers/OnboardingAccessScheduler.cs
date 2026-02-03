using Application.CQRSInterfaces;
using Application.Interfaces;
using Application.Results;
using Application.UseCases.CandidateProcesses.Commands.TransferToOnboarding;
using Application.UseCases.OnboardingAccessRequests.Queries.GetDueCandidates;
using Contracts.NotificationDtos;
using Domain.Entities;

namespace Scheduler.Schedulers;

public class OnboardingAccessScheduler(
    IQueryHandler<List<User>, GetDueCandidatesQuery> getDueCandidatesQueryHandler,
    ICommandHandlerWithResult<TransferToOnboardingCommand, CandidateProcess> transferToOnboardingHandler,
    INotificationPublisher notificationPublisher
)
{
    public async Task ProcessDueRequests()
    {
        DateTime utcNow = DateTime.UtcNow;
        GetDueCandidatesQuery query = new()
        {
            UtcNow = utcNow
        };

        Result<List<User>> usersResult = await getDueCandidatesQueryHandler.HandleAsync( query );

        if ( !usersResult.IsSuccess || usersResult.Value == null || usersResult.Value.Count == 0 )
        {
            return;
        }

        foreach ( User user in usersResult.Value )
        {
            TransferToOnboardingCommand command = new()
            {
                CandidateId = user.Id
            };
            
            Result<CandidateProcess> transferResult = await transferToOnboardingHandler.HandleAsync( command );

            if ( transferResult.IsSuccess )
            {
                long? telegramId = user.ContactInfo?.TelegramId;
                string? firstName = user.ContactInfo?.FirstName;
                DateTime? onboardingAccessTimeUtc = user.OnboardingAccessTimeUtc;

                if ( telegramId.HasValue && 
                     !string.IsNullOrWhiteSpace( firstName ) &&
                     onboardingAccessTimeUtc.HasValue )
                {
                    FeedbackReminderRequest dto = new()
                    {
                        TelegramId = telegramId.Value,
                        FirstName = firstName,
                        AccessTimeUtc = onboardingAccessTimeUtc.Value
                    };

                    await notificationPublisher.PublishOnboardingGrantedAsync( dto );
                }
            }
        }
    }
}