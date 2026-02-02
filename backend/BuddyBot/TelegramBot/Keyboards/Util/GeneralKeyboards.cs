using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Util;
public static class GeneralKeyboards
{
    public static InlineKeyboardMarkup ContinueCandidate( string callbackData )
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "🚀 Продолжаем путь!",
                callbackData: callbackData
            )
        );
    }

    public static InlineKeyboardMarkup ContactHR( string hrLink )
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithUrl( 
                text: "Связаться с HR",
                url: hrLink 
            )
        );
    }
}
