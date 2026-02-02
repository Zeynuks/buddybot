using AppUser = Domain.Entities.User;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Keyboards.Util;

namespace TelegramBot.Helpers;
public static class BotMessageHelper
{
    public static async Task SendErrorMessage(
        ITelegramBotClient botClient,
        long chatId,
        string message,
        CancellationToken cancellationToken,
        AppUser? candidate = null )
    {
        InlineKeyboardMarkup? replyMarkup = null;

        if ( candidate != null && candidate.HRs != null && candidate.HRs.Count > 0 )
        {
            AppUser? hr = candidate.HRs.FirstOrDefault();
            string? hrContact = hr?.ContactInfo?.TelegramContact;

            if ( !string.IsNullOrWhiteSpace( hrContact ) && hrContact.StartsWith( "https://t.me/" ) )
            {
                replyMarkup = GeneralKeyboards.ContactHR( hrContact );
            }
        }

        await botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken
        );
    }
}