using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.BackgroundServices.Abstract;
public abstract class ReceiverServiceBase<TUpdateHandler>( ITelegramBotClient botClient, TUpdateHandler updateHandler) : IReceiverService
    where TUpdateHandler : IUpdateHandler
{
    public virtual async Task ReceiveAsync( CancellationToken stoppingToken )
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [],
            DropPendingUpdates = true
        };

        User me = await botClient.GetMe( stoppingToken );

        await botClient.ReceiveAsync( updateHandler: updateHandler, receiverOptions: receiverOptions, cancellationToken: stoppingToken );
    }
}