using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;
using TelegramBot.Messages;

namespace TelegramBot.Handlers.Candidate.PersonalArea;
public class PersonalAreaLinksHandler : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "PersonalAreaLinks";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        await botClient.SendSticker(
            chatId: callbackQuery.Message.Chat.Id,
            sticker: StickerIds.PersonalAreaLinksSticker,
            cancellationToken: cancellationToken
        );
        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: PersonalAreaMessages.PersonalAreaLinks,
            replyMarkup: Inline.PersonalAreaLinks(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}
