using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Keyboards.Candidate.Feedback;
using TelegramBot.Messages;

namespace TelegramBot.Notifiers;
public class FeedbackNotifier( ITelegramBotClient botClient )
{
    public async Task NotifyPreboarding( long telegramId, string firstName )
    {
        await botClient.SendMessage(
            chatId: telegramId,
            text: FeedbackMessages.PreboardingFeedbackRating( firstName ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.PreboardingRating()
        );
    }

    public async Task NotifyOnboarding( long telegramId, string firstName )
    {
        await botClient.SendMessage(
            chatId: telegramId,
            text: FeedbackMessages.OnboardingFeedbackRating( firstName ),
            parseMode: ParseMode.Html,
            replyMarkup: Inline.OnboardingRating()
        );
    }
}