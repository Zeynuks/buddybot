using Domain.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Candidate.Onboarding;
public static class Inline
{
    public static InlineKeyboardMarkup OnboardingPendingStart()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Начать онбординг",
                callbackData: StepKind.MeetTeamIntro.ToString()
            )
        );
    }

    public static InlineKeyboardMarkup OnboardingMeetTeamIntro()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Познакомиться с командой",
                callbackData: StepKind.MeetHead.ToString()
            )
        );
    }

    public static InlineKeyboardMarkup OnboardingMeetHead()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Кто мой наставник?",
                callbackData: StepKind.MeetMentor.ToString()
            )
        );
    }

    public static InlineKeyboardMarkup OnboardingMeetMentor()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Спасибо, ознакомился!",
                callbackData: StepKind.CompanyPolicies.ToString()
            )
        );
    }

    public static InlineKeyboardMarkup OnboardingCompanyPolicies()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Все понятно!",
                callbackData: "PersonalAreaHome"
            )
        );
    }
}
