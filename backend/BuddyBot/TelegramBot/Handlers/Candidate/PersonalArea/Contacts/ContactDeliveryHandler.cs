using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;

namespace TelegramBot.Handlers.Candidate.PersonalArea.Contacts;
public class ContactDeliveryHandler : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "ContactDelivery";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        string imagePath = Path.Combine( AppContext.BaseDirectory, "Static", "Images", "Аня Моисеева.jpeg" );
        string fullName = "Аня Моисеева";
        string teamsUrl = "https://teams.microsoft.com/l/chat/0/0?users=anna.moiseeva@travelline.ru";

        await using ( var stream = File.OpenRead( imagePath ) )
        {
            await botClient.SendPhoto(
                chatId: callbackQuery.Message.Chat.Id,
                photo: new InputFileStream( stream, "Аня Моисеева.jpeg" ),
                caption: $"<b>{fullName}</b>",
                replyMarkup: Inline.ContactCardKeyboard( teamsUrl ),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
    }
}