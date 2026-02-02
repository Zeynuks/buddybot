using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Admin;
public static class Inline
{
    public static InlineKeyboardMarkup ApproveOnboardingKeyboard()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Выдать доступ",
                    callbackData: "temp"
                ),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отклонить запрос",
                    callbackData: "temp"
                )
            }
        } );
    }
}
