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

namespace TelegramBot.Handlers.Candidate.Preboarding;
public class ResolveQuestionsHandler( UserService userService, CandidateService candidateService ) : IStepHandler
{
    public StepKind Step => StepKind.ResolveQuestions;

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

        (string hrName, string hrLink) = GetHrInfo( candidate );

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: PreboardingMessages.ResolveQuestions( hrName, hrLink ),
            replyMarkup: Inline.ResolveQuestions( hrLink ),
            linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    private (string hrName, string hrLink) GetHrInfo( AppUser candidate )
    {
        AppUser? hr = candidate.GetHRs().FirstOrDefault();

        string firstName = hr?.ContactInfo?.FirstName ?? "HR";
        string lastName = hr?.ContactInfo?.LastName ?? string.Empty;
        string name = string.IsNullOrWhiteSpace( lastName )
            ? firstName
            : $"{firstName} {lastName}";

        string link = hr?.ContactInfo?.TelegramContact ?? string.Empty;

        return (name, link);
    }
}