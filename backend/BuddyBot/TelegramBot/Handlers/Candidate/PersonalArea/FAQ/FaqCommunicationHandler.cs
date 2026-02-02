using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Messages;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;

namespace TelegramBot.Handlers.Candidate.PersonalArea.FAQ;
public class FaqCommunicationHandler : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "FaqCommunication";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        await botClient.SendMessage(
            callbackQuery.Message.Chat.Id,
            text: PersonalAreaMessages.FaqCommunication,
            replyMarkup: Inline.PersonalAreaFaqBackHome(),
            linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}