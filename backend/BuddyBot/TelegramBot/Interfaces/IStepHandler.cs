using Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Interfaces;
public interface IStepHandler
{
    StepKind Step { get; }
    Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken );
}