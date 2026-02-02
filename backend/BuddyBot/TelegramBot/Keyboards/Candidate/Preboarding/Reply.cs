using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Candidate.Preboarding;
public static class Reply
{
    public static ReplyKeyboardMarkup ShareContact()
    {
        return new ReplyKeyboardMarkup(
            KeyboardButton.WithRequestContact( "📱 Поделиться своим контактом" )
        )
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }
}
