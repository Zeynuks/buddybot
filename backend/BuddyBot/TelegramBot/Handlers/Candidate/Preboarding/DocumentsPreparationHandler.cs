using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Messages;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.Preboarding;
using TelegramBot.Helpers;
using TelegramBot.Notifiers;

namespace TelegramBot.Handlers.Candidate.Preboarding;
public class DocumentsPreparationHandler( UserService userService, CandidateService candidateService, HRNotifier hrNotifier ) : IStepHandler
{
    public StepKind Step => StepKind.DocumentsPreparation;

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

        await candidateService.GoNextStep( candidate.Id, ProcessKind.Preboarding, callbackQuery.Data );

        string candidateFirstName = GetCandidateFirstName( candidate );
        string candidateFullName = GetCandidateFullName( candidate );

        await hrNotifier.NotifyCandidateHrs( candidate, HRMessages.OfferAcceptedNotification ( candidateFullName ), cancellationToken );

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: PreboardingMessages.DocumentsPreparation( candidateFirstName ),
            replyMarkup: Inline.DocumentsPreparation(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    private string GetCandidateFirstName( AppUser candidate )
    {
        return candidate.ContactInfo?.FirstName ?? "кандидат";
    }

    private string GetCandidateFullName( AppUser candidate )
    {
        string firstName = candidate.ContactInfo?.FirstName ?? "";
        string lastName = candidate.ContactInfo?.LastName ?? "";
        string fullName = $"{firstName} {lastName}".Trim();
        return string.IsNullOrWhiteSpace( fullName ) ? "Кандидат" : fullName;
    }
}