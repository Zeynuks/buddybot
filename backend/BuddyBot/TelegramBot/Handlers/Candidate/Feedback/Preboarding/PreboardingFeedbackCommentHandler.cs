using AppUser = Domain.Entities.User;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions;
using TelegramBot.Helpers;
using TelegramBot.Interfaces;
using TelegramBot.Messages;
using TelegramBot.Services;
using Domain.Enums;

namespace TelegramBot.Handlers.Candidate.Feedback.Preboarding;
public class PreboardingFeedbackCommentHandler( UserService userService, FeedbackService feedbackService ) : IMessageHandler
{
    public bool CanHandle( string messageText )
    {
        if ( messageText.StartsWith( "/" ) )
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace( messageText );
    }

    public async Task<bool> HandleAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( !await botClient.EnsureUserMessageOrSendError( message, cancellationToken ) )
        {
            return false;
        }
        long telegramId = message.From!.Id;

        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, message.Chat.Id, ErrorMessages.CandidateNotFound, cancellationToken );
            return true;
        }

        var draftFeedback = await feedbackService.GetDraftFeedback( candidate.Id, ProcessKind.Preboarding );
        if ( draftFeedback == null )
        {
            return false;
        }

        var confirmResult = await feedbackService.ConfirmFeedback( draftFeedback.Id, message.Text ?? string.Empty );

        if ( !confirmResult.IsSuccess )
        {
            await BotMessageHelper.SendErrorMessage( botClient, message.Chat.Id, ErrorMessages.FeedbackConfirmationFailed, cancellationToken );
            return true;
        }

        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: FeedbackMessages.FeedbackThanks(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );

        return true;
    }
}
