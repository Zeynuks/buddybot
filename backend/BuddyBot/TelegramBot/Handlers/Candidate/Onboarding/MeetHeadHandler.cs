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
public class MeetHeadHandler( UserService userService, CandidateService candidateService, MediaService mediaService ) : IStepHandler
{
    public StepKind Step => StepKind.MeetHead;

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

        await SendHeadInfo( candidate, callbackQuery.Message.Chat.Id, botClient, cancellationToken );
    }

    private async Task SendHeadInfo( AppUser candidate, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        AppUser? head = candidate.Team?.Leader;
        if ( head is null || head.ContactInfo is null )
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: "Руководитель отдела пока не назначен. Не переживай, HR этим скоро займётся =).",
                replyMarkup: Inline.OnboardingMeetHead(),
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
                    chatId: chatId,
                    photo: photoStream,
                    caption: OnboardingMessages.MeetHead( headName, teamsUrl ),
                    parseMode: ParseMode.Html,
                    replyMarkup: Inline.OnboardingMeetHead(),
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
                    caption: OnboardingMessages.MeetHead( headName, teamsUrl ),
                    replyMarkup: Inline.OnboardingMeetHead(),
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
