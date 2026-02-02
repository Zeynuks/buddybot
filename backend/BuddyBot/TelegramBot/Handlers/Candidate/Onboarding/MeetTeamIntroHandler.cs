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
public class MeetTeamIntroHandler( UserService userService, CandidateService candidateService ) : IStepHandler
{
    public StepKind Step => StepKind.MeetTeamIntro;

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

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: OnboardingMessages.MeetTeamIntro(),
            replyMarkup: Inline.OnboardingMeetTeamIntro(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}
