using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Messages;
using TelegramBot.Extensions;
using TelegramBot.Helpers;
using TelegramBot.Notifiers;
using TelegramBot.Keyboards.Admin;

namespace TelegramBot.Handlers.Candidate.Preboarding;
public class WaitAdminApproveHandler( UserService userService, CandidateService candidateService, HRNotifier hrNotifier, NotificationService notificationService) : IStepHandler
{
    public StepKind Step => StepKind.WaitAdminApprove;

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }
        long telegramId = callbackQuery.From.Id;

        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.CandidateNotFound, cancellationToken );
            return;
        }

        bool isNewRequest = await candidateService.RequestOnboardingAccess( candidate.Id );
        string messageText = isNewRequest
            ? PreboardingMessages.WaitAdminApprove
            : PreboardingMessages.AlreadyRequestedAccess;

        if ( isNewRequest )
        {
            await candidateService.GoNextStep( candidate.Id, ProcessKind.Preboarding, callbackQuery.Data );
            notificationService.SchedulePreboardingFeedback( telegramId, candidate.ContactInfo?.FirstName );

            string candidateName = $"{candidate.ContactInfo?.FirstName} {candidate.ContactInfo?.LastName}".Trim();
            await hrNotifier.NotifyCandidateHrs( candidate, HRMessages.OnboardingAccessRequestedNotification( candidateName ), cancellationToken, Inline.ApproveOnboardingKeyboard() );
        
            await botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: PreboardingMessages.WaitAdminApprove,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: messageText,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
    }
}
