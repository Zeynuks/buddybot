using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;

namespace TelegramBot.Handlers.Candidate.PersonalArea.Contacts;
public class ContactBenefitsHandler : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "ContactBenefits";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        string imagePath = Path.Combine( AppContext.BaseDirectory, "Static", "Images", "Марина Софронова.jpeg" );
        string fullName = "Марина Софронова";
        string teamsUrl = "https://teams.microsoft.com/l/chat/0/0?users=marina.sofronova@travelline.ru";

        await using ( var stream = File.OpenRead( imagePath ) )
        {
            await botClient.SendPhoto(
                chatId: callbackQuery.Message.Chat.Id,
                photo: new InputFileStream( stream, "Марина Софронова.jpeg" ),
                caption: $"<b>{fullName}</b>",
                replyMarkup: Inline.ContactCardKeyboard( teamsUrl ),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
    }
}