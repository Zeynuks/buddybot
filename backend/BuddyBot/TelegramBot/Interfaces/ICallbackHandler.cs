using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Interfaces;
public interface ICallbackHandler
{
    bool CanHandle( string callbackData );
    Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken );
}