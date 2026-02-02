using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Messages;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.Onboarding;
using TelegramBot.Helpers;

namespace TelegramBot.Handlers.Candidate.Onboarding;
public class MeetMentorHandler( UserService userService, CandidateService candidateService, MediaService mediaService ) : IStepHandler
{
    public StepKind Step => StepKind.MeetMentor;

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

        await candidateService.GoNextStep( candidate.Id, ProcessKind.Onboarding, callbackQuery.Data );

        await SendMentorInfo( candidate, callbackQuery.Message.Chat.Id, botClient, cancellationToken );
    }

    private async Task SendMentorInfo(
        AppUser candidate,
        long chatId,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken )
    {
        AppUser? mentor = candidate.Mentors?.FirstOrDefault();
        if ( mentor is null || mentor.ContactInfo is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, chatId, ErrorMessages.MentorNotFound, cancellationToken, candidate );
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
                    chatId: chatId,
                    photo: photoStream,
                    caption: OnboardingMessages.MeetMentor( mentorName, teamsUrl ),
                    parseMode: ParseMode.Html,
                    replyMarkup: Inline.OnboardingMeetMentor(),
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
                    chatId: chatId,
                    photo: new InputFileStream( stream, "AccountImage.jpg" ),
                    caption: OnboardingMessages.MeetMentor( mentorName, teamsUrl ),
                    replyMarkup: Inline.OnboardingMeetMentor(),
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
