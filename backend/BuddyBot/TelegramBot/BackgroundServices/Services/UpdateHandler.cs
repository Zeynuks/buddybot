using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBot.Routers;

namespace TelegramBot.BackgroundServices.Services;
public class UpdateHandler( IServiceProvider serviceProvider ) : IUpdateHandler
{
    public async Task HandleUpdateAsync( ITelegramBotClient botClient, Update update, CancellationToken cancellationToken )
    {
        using var scope = serviceProvider.CreateScope();
        MessageRouter messageRouter = scope.ServiceProvider.GetRequiredService<MessageRouter>();
        StepRouter stepRouter = scope.ServiceProvider.GetRequiredService<StepRouter>();

        try
        {
            if ( update.Type == UpdateType.Message && update.Message != null )
            {
                await messageRouter.RouteAsync( update.Message, botClient, cancellationToken );
            }
            else if ( update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data is not null )
            {
                await stepRouter.RouteAsync( update.CallbackQuery, botClient, cancellationToken );
            }
        }
        catch ( Exception ex )
        {
            await HandleErrorAsync( botClient, ex, HandleErrorSource.PollingError, cancellationToken );
        }
    }

    public Task HandleErrorAsync( ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken )
    {
        string error = exception switch
        {
            ApiRequestException apiEx =>
                $"Telegram API Error:\n[{apiEx.ErrorCode}]\n{apiEx.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine( error );
        return Task.CompletedTask;
    }
}
