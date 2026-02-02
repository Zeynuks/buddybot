using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.BackgroundServices.Services;
public class StartupBotInitializer( ITelegramBotClient botClient ) 
    : IHostedService
{
    public async Task StartAsync( CancellationToken cancellationToken )
    {
        List<BotCommand> commands = new List<BotCommand>
        {
            new() { Command = "start", Description = "Начало работы с ботом" },
            new() { Command = "home", Description = "Личный кабинет" },
            new() { Command = "reset", Description = "Сбросить прогресс" },
        };

        await botClient.SetMyCommands( commands, cancellationToken: cancellationToken );
    }

    public Task StopAsync( CancellationToken cancellationToken ) => Task.CompletedTask;
}