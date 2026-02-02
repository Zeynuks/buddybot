using AppUser = Domain.Entities.User;

using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TelegramBot.Messages;
using TelegramBot.Keyboards.Candidate.Onboarding;
using Domain.Enums;
using TelegramBot.Services;
using TelegramBot.Helpers;

namespace TelegramBot.Notifiers;
public class OnboardingNotifier( ITelegramBotClient botClient, CandidateService candidateService, UserService userService )
{
    public async Task NotifyGranted( long telegramId, string firstName )
    {
        await botClient.SendMessage(
            chatId: telegramId,
            text: OnboardingMessages.OnboardingPendingStart( firstName ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingPendingStart()
        );
    }

    public async Task NotifyStartReminder( long telegramId, string firstName )
    {
        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            return;
        }

        var process = await candidateService.GetCandidateProcess( candidate.Id, ProcessKind.Onboarding );

        if ( process == null || process.CurrentStep != StepKind.OnboardingPendingStart )
        {
            return;
        }

        await botClient.SendMessage(
            chatId: telegramId,
            text: OnboardingMessages.OnboardingReminder( firstName ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingPendingStart()
        );
    }
}