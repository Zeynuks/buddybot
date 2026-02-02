using AppUser = Domain.Entities.User;

using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Extensions;
using Telegram.Bot.Types.Enums;
using TelegramBot.Services;
using Domain.Enums;
using TelegramBot.Helpers;
using TelegramBot.Messages;

namespace TelegramBot.Handlers.Candidate.Feedback.Preboarding;
public class PreboardingFeedbackNoCommentHandler( UserService userService, FeedbackService feedbackService ) : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "Feedback:Preboarding:NoSuggestions";

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

        var draftFeedback = await feedbackService.GetDraftFeedback( candidate.Id, ProcessKind.Preboarding );
        if ( draftFeedback == null )
        {
            return;
        }

        var confirmResult = await feedbackService.ConfirmFeedback( draftFeedback.Id, string.Empty );

        if ( !confirmResult.IsSuccess )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.FeedbackConfirmationFailed, cancellationToken );
            return;
        }

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: FeedbackMessages.FeedbackThanks(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}