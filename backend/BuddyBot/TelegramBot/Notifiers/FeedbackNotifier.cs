using Contracts.NotificationDtos;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Keyboards.Candidate.Feedback;
using TelegramBot.Messages;

namespace TelegramBot.Notifiers;

public class FeedbackNotifier( ITelegramBotClient botClient )
{
    public async Task NotifyPreboarding( FeedbackReminderRequest request )
    {
        await botClient.SendMessage(
            chatId: request.TelegramId,
            text: FeedbackMessages.PreboardingFeedbackRating( request.FirstName ?? string.Empty ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.PreboardingRating()
        );
    }

    public async Task NotifyOnboarding( FeedbackReminderRequest request )
    {
        await botClient.SendMessage(
            chatId: request.TelegramId,
            text: FeedbackMessages.OnboardingFeedbackRating( request.FirstName ?? string.Empty ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingRating()
        );
    }
}