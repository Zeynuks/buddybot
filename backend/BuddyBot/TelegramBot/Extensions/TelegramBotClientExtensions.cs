using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Messages;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Extensions;
public static class TelegramBotClientExtensions
{
    public static async Task RemoveInlineKeyboard( 
        this ITelegramBotClient botClient,
        Message? message, 
        CancellationToken cancellationToken )
    {
        if ( message is not null )
        {
            await botClient.EditMessageReplyMarkup(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                replyMarkup: null,
                cancellationToken: cancellationToken
            );
        }
    }

    public static async Task<bool> EnsureUserMessageOrSendError(
        this ITelegramBotClient botClient,
        Message message,
        CancellationToken cancellationToken )
    {
        if ( message.From is null )
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: ErrorMessages.TelegramUserNotFound,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
            return false;
        }
        return true;
    }
}
