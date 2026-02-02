using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Interfaces;
public interface IMessageHandler
{
    bool CanHandle( string messageText );
    Task<bool> HandleAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken );
}
