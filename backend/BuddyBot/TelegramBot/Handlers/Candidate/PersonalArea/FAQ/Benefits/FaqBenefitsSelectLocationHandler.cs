using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Messages;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;

namespace TelegramBot.Handlers.Candidate.PersonalArea.FAQ.Benefits;

public class FaqBenefitsSelectLocationHandler : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "FaqBenefitsSelectLocation";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        await botClient.SendSticker(
            chatId: callbackQuery.Message.Chat.Id,
            sticker: StickerIds.FaqBenefitsSticker,
            cancellationToken: cancellationToken
        );
        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: PersonalAreaMessages.FaqBenefitsSelectLocation,
            replyMarkup: Inline.FaqBenefitsSelectLocation(),
            linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}
