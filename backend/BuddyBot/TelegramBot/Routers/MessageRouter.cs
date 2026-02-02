using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;

namespace TelegramBot.Routers;
public class MessageRouter( IEnumerable<IMessageHandler> handlers )
{
    private readonly List<IMessageHandler> Handlers = handlers.ToList();

    public async Task RouteAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( !string.IsNullOrEmpty( message.Text ) )
        {
            foreach ( var handler in Handlers )
            {
                if ( handler.CanHandle( message.Text! ) )
                {
                    bool handled = await handler.HandleAsync( message, botClient, cancellationToken );
                    if ( handled )
                        return;
                }
            }
        }
    }
}
