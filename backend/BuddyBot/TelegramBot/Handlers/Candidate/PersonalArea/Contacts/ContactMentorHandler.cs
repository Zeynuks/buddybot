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
public class ContactMentorHandler( UserService userService, MediaService mediaService ) : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "ContactMentor";

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

        AppUser? mentor = candidate.Mentors?.FirstOrDefault();
        if ( mentor is null || mentor.ContactInfo is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.MentorNotFound, cancellationToken, candidate );
            return;
        }

        string mentorName = GetFullName( mentor );
        string teamsUrl = mentor.ContactInfo.MicrosoftTeamsUrl ?? "";
        string? photoId = mentor.ContactInfo.MentorPhotoUrl;

        bool photoSent = false;

        if ( !string.IsNullOrEmpty( photoId ) )
        {
            var photoStream = await mediaService.DownloadMedia( photoId );
            if ( photoStream != null )
            {
                await botClient.SendPhoto(
                    chatId: callbackQuery.Message.Chat.Id,
                    photo: photoStream,
                    caption: $"<b>{mentorName}</b>",
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
                    caption: $"<b>{mentorName}</b>",
                    replyMarkup: Inline.ContactCardKeyboard( teamsUrl ),
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
            }
        }
    }
    private string GetFullName( AppUser mentor )
    {
        string firstName = mentor.ContactInfo?.FirstName ?? "";
        string lastName = mentor.ContactInfo?.LastName ?? "";
        string fullName = $"{firstName} {lastName}".Trim();
        return string.IsNullOrWhiteSpace( fullName ) ? "Наставник" : fullName;
    }
}