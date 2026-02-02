using AppUser = Domain.Entities.User;

using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Extensions;

namespace TelegramBot.Handlers.Admin.SpecialCommands;
public class ResetCandidateProcessHandler( CandidateService candidateService, UserService userService ) : IMessageHandler
{
    public bool CanHandle( string messageText ) => messageText.StartsWith( "/reset" );

    public async Task<bool> HandleAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( !await botClient.EnsureUserMessageOrSendError( message, cancellationToken ) )
        {
            return false;
        }
        long telegramId = message.From!.Id;
        AppUser? user = await userService.GetUserByTelegramId( telegramId );

        if ( user == null || !user.IsCandidate() )
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Сбросить прогресс можно только кандидату.",
                cancellationToken: cancellationToken
            );
            return true;
        }

        var result = await candidateService.ResetCandidateProcess( user.Id );

        if ( result.IsSuccess )
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Ваш прогресс сброшен. Начните прохождение с самого начала.",
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: $"Ошибка сброса: {result.Error?.Message ?? "Не удалось сбросить прогресс."}",
                cancellationToken: cancellationToken
            );
        }

        return true;
    }
}