using AppUser = Domain.Entities.User;

using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.PersonalArea;
using TelegramBot.Services;
using TelegramBot.Helpers;
using TelegramBot.Messages;

namespace TelegramBot.Handlers.Candidate.PersonalArea.Contacts;
public class ContactHeadHandler( UserService userService, MediaService mediaService ) : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "ContactHead";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }
        long telegramId = callbackQuery.From.Id;

        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.CandidateNotFound, cancellationToken );
            return;
        }

        AppUser? head = candidate.Team?.Leader;
        if ( head is null || head.ContactInfo is null )
        {
            await botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Руководитель отдела пока не назначен. Не переживай, HR этим скоро займётся =).",
                replyMarkup: Inline.ContactCardKeyboard(),
                cancellationToken: cancellationToken
            );
            return;
        }

        string headName = GetFullName( head );
        string teamsUrl = head.ContactInfo.MicrosoftTeamsUrl ?? "";
        string? photoId = head.ContactInfo.MentorPhotoUrl;

        bool photoSent = false;

        if ( !string.IsNullOrEmpty( photoId ) )
        {
            var photoStream = await mediaService.DownloadMedia( photoId );
            if ( photoStream != null )
            {
                await botClient.SendPhoto(
                    chatId: callbackQuery.Message.Chat.Id,
                    photo: photoStream,
                    caption: $"<b>{headName}</b>",
                    replyMarkup: Inline.ContactCardKeyboard( teamsUrl ),
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
                photoSent = true;
            }
        }

        if ( !photoSent )
        {
            string imagePath = Path.Combine( AppContext.BaseDirectory, "Static", "Images", "AccountImage.jpg" );
            await using ( var stream = File.OpenRead( imagePath ) )
            {
                await botClient.SendPhoto(
                    chatId: callbackQuery.Message.Chat.Id,
                    photo: new InputFileStream( stream, "AccountImage.jpg" ),
                    caption: $"<b>{headName}</b>",
                    replyMarkup: Inline.ContactCardKeyboard( teamsUrl ),
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
            }
        }
    }

    private string GetFullName( AppUser head )
    {
        string firstName = head.ContactInfo?.FirstName ?? "";
        string lastName = head.ContactInfo?.LastName ?? "";
        string fullName = $"{firstName} {lastName}".Trim();
        return string.IsNullOrWhiteSpace( fullName ) ? "Руководитель" : fullName;
    }
}