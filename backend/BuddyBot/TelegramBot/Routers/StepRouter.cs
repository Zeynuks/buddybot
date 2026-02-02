using Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;

namespace TelegramBot.Routers;
public class StepRouter(
    IEnumerable<IStepHandler> stepHandlers,
    IEnumerable<ICallbackHandler> callbackHandlers )
{
    public Dictionary<StepKind, IStepHandler> StepHandlers = stepHandlers.ToDictionary( h => h.Step, h => h );
    public List<ICallbackHandler> CallbackHandlers = callbackHandlers.ToList();


    public async Task RouteAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( Enum.TryParse<StepKind>( callbackQuery.Data, out var stepKind )
            && StepHandlers.TryGetValue( stepKind, out var stepHandler ) )
        {
            await stepHandler.HandleAsync( callbackQuery, botClient, cancellationToken );
            return;
        }

        var handler = CallbackHandlers.FirstOrDefault( h => h.CanHandle( callbackQuery.Data! ) );
        if ( handler != null )
        {
            await handler.HandleAsync( callbackQuery, botClient, cancellationToken );
            return;
        }

        if ( callbackQuery.Message is not null )
        {
            await botClient.SendMessage(
                callbackQuery.Message.Chat.Id,
                "Извините, этот шаг пока не реализован.",
                cancellationToken: cancellationToken
            );
        }
    }
}