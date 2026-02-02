using AppUser = Domain.Entities.User;

using Domain.Entities;
using Domain.Enums;
using Application.Results;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Messages;
using TelegramBot.Helpers;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.Preboarding;
using TelegramBot.Keyboards.Util;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Notifiers;

namespace TelegramBot.Handlers;

public class StartHandler( UserService userService, CandidateService candidateService, HRNotifier hrNotifier ) : IMessageHandler
{
    public bool CanHandle( string messageText ) => messageText.StartsWith( "/start" );

    public async Task<bool> HandleAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( !await botClient.EnsureUserMessageOrSendError( message, cancellationToken ) )
        {
            return false;
        }
        long telegramId = message.From!.Id; 
        AppUser? user = await userService.GetUserByTelegramId( telegramId );
        if ( user != null )
        {
            if ( user.IsHR() )
            {
                await HandleHrStart( user, botClient, message, cancellationToken );
                return true;
            }

            if ( user.IsCandidate() )
            {
                await HandleCandidateStart( user, botClient, message, cancellationToken );
                return true;
            }
            return true;
        }

        await HandleTokenActivation( message, botClient, telegramId, cancellationToken );
        return true;
    }

    private async Task HandleHrStart( AppUser user, ITelegramBotClient botClient, Message message, CancellationToken cancellationToken )
    {
        string hrName = user.ContactInfo?.FirstName ?? message.From?.FirstName ?? "HR";
        await SendWelcome( botClient, message.Chat.Id, HRMessages.Welcome( hrName ), cancellationToken );
    }

    private async Task HandleCandidateStart( AppUser user, ITelegramBotClient botClient, Message message, CancellationToken cancellationToken )
    {
        CandidateProcess? activeProcess = await candidateService.GetActiveProcess( user.Id );
        if ( activeProcess == null || activeProcess.CurrentStep == StepKind.PreboardingStart )
        {
            string welcomeText = PreboardingMessages.StartCandidate( user.ContactInfo?.FirstName ?? message.From?.FirstName ?? "Кандидат" );
            await SendWelcome( botClient, message.Chat.Id, welcomeText, cancellationToken,Inline.StartCandidate() );
        }
        else
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: GeneralMessages.ContinueCandidate,
                replyMarkup: GeneralKeyboards.ContinueCandidate( activeProcess.CurrentStep.ToString()),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
        return;
    }

    private async Task HandleTokenActivation( Message message, ITelegramBotClient botClient, long telegramId, CancellationToken cancellationToken )
    {
        Guid token;
        TokenParseResult parseResult = InviteTokenHelper.TryParseTokenFromStartCommand( message.Text, out token );

        if ( parseResult == TokenParseResult.NoToken )
        {
            await BotMessageHelper.SendErrorMessage( botClient, message.Chat.Id, ErrorMessages.RegistrationTokenRequired, cancellationToken );
            return;
        }
        if ( parseResult == TokenParseResult.InvalidTokenFormat )
        {
            await BotMessageHelper.SendErrorMessage( botClient, message.Chat.Id, ErrorMessages.InvalidTokenFormat, cancellationToken );
            return;
        }

        Result<AppUser> result = await userService.ActivateUser( token, telegramId );

        if ( !result.IsSuccess )
        {
            await BotMessageHelper.SendErrorMessage( botClient, message.Chat.Id, result.Error?.Message ?? ErrorMessages.ActivationFailed, cancellationToken );
            return;
        }

        AppUser activatedUser = result.Value!;

        if ( activatedUser.IsCandidate() )
        {
            string candidateName = $"{activatedUser.ContactInfo?.FirstName ?? ""} {activatedUser.ContactInfo?.LastName ?? ""}".Trim();
            await hrNotifier.NotifyCandidateHrs( activatedUser, HRMessages.NewCandidateNotification( candidateName ), cancellationToken );

            string welcomeText = PreboardingMessages.StartCandidate(
                activatedUser.ContactInfo?.FirstName ?? message.From?.FirstName ?? "Кандидат"
            );
            await SendWelcome( botClient, message.Chat.Id, welcomeText, cancellationToken, Inline.StartCandidate() );
        }
        else if ( activatedUser.IsHR() )
        {
            string hrName = activatedUser.ContactInfo?.FirstName ?? message.From?.FirstName ?? "HR";
            await SendWelcome( botClient, message.Chat.Id, HRMessages.Welcome( hrName ), cancellationToken);
        }
    }

    private async Task SendWelcome( ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken, ReplyMarkup? replyMarkup = null )
    {
        await botClient.SendSticker(
            chatId: chatId,
            sticker: StickerIds.StartSticker,
            cancellationToken: cancellationToken
        );
        await botClient.SendMessage(
            chatId: chatId,
            text: messageText,
            replyMarkup: replyMarkup,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}