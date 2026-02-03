using Contracts.NotificationDtos;
using Domain.Entities;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TelegramBot.Messages;
using TelegramBot.Keyboards.Candidate.Onboarding;
using Domain.Enums;
using TelegramBot.Services;

namespace TelegramBot.Notifiers;

public class OnboardingNotifier(
    ITelegramBotClient botClient,
    CandidateService candidateService,
    UserService userService)
{
    public async Task NotifyGranted(FeedbackReminderRequest request)
    {
        await botClient.SendMessage(
            chatId: request.TelegramId,
            text: OnboardingMessages.OnboardingPendingStart(request.FirstName ?? string.Empty),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingPendingStart()
        );
    }

    public async Task NotifyStartReminder(FeedbackReminderRequest request)
    {
        User? candidate = await userService.GetUserByTelegramId(request.TelegramId);
        if (candidate is null)
        {
            return;
        }

        var process = await candidateService.GetCandidateProcess(candidate.Id, ProcessKind.Onboarding);

        if (process == null || process.CurrentStep != StepKind.OnboardingPendingStart)
        {
            return;
        }

        await botClient.SendMessage(
            chatId: request.TelegramId,
            text: OnboardingMessages.OnboardingReminder(request.FirstName ?? string.Empty),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingPendingStart()
        );
    }
}