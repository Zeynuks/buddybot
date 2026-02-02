using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Helpers;
using TelegramBot.Interfaces;
using TelegramBot.Messages;
using TelegramBot.Services;
using TelegramBot.Extensions;

namespace TelegramBot.Handlers.Candidate.Feedback.Onboarding;
public class OnboardingFeedbackNoCommentHandler( UserService userService, FeedbackService feedbackService ) : ICallbackHandler
{
    public bool CanHandle( string callbackData ) => callbackData == "Feedback:Onboarding:NoSuggestions";

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

        var draftFeedback = await feedbackService.GetDraftFeedback( candidate.Id, ProcessKind.Onboarding );
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
