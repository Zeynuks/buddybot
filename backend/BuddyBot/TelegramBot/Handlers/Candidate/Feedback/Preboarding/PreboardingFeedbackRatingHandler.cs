using AppUser = Domain.Entities.User;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions;
using TelegramBot.Helpers;
using TelegramBot.Interfaces;
using TelegramBot.Keyboards.Candidate.Feedback;
using TelegramBot.Messages;
using TelegramBot.Services;
using Domain.Enums;

namespace TelegramBot.Handlers.Candidate.Feedback.Preboarding;
public class PreboardingFeedbackRatingHandler( UserService userService, FeedbackService feedbackService ) : ICallbackHandler
{
    public bool CanHandle( string callbackData )
    {
        return callbackData.StartsWith( "Feedback:Preboarding:" );
    }

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }
        long telegramId = callbackQuery.From.Id;

        int rating;
        if ( !FeedbackRatingHelper.TryParseFeedbackRating( callbackQuery.Data, out rating ) )
        {
            return;
        }

        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.CandidateNotFound, cancellationToken );
            return;
        }

        var draftFeedback = await feedbackService.CreateFeedback( candidate.Id, ProcessKind.Preboarding, rating );
        if ( !draftFeedback.IsSuccess )
        {
            await BotMessageHelper.SendErrorMessage( botClient, callbackQuery.Message.Chat.Id, ErrorMessages.FeedbackCreationFailed, cancellationToken );
            return;
        }

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: FeedbackMessages.PreboardingFeedbackComment(),
            replyMarkup: Inline.PreboardingComment(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}
