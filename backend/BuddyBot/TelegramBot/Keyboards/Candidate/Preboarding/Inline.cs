using Domain.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Candidate.Preboarding;
public static class Inline
{
    public static InlineKeyboardMarkup StartCandidate()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Начать пребординг",
                callbackData: StepKind.PreboardingWelcome.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup PreboardingWelcome()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Понял, поехали!",
                callbackData: StepKind.CompanyIntro.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup CompanyIntro()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Что дальше?",
                callbackData: StepKind.OfferDecision.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup OfferDecision()
    {
        return new InlineKeyboardMarkup( new[]
       {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "У вас классно, я принимаю оффер!",
                    callbackData: StepKind.DocumentsPreparation.ToString()
                )
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Ознакомился, но ещё хочу подумать",
                    callbackData: StepKind.ContactHR.ToString()
                )
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Часто задаваемые вопросы",
                    callbackData: StepKind.PreboardingFAQ.ToString()
                )
            }
        } );
    }
    public static InlineKeyboardMarkup ContactHR( string? hrLink )
    {
        if ( string.IsNullOrWhiteSpace( hrLink ) )
        {
            return new InlineKeyboardMarkup( new[]
            {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Я готов! Принимаю оффер",
                    callbackData: StepKind.DocumentsPreparation.ToString()
                )
            }
        } );
        }

        return new InlineKeyboardMarkup( new[]
        {
        new[]
        {
            InlineKeyboardButton.WithUrl(
                text: "Написать HR",
                url: hrLink
            )
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(
                text: "Я готов! Принимаю оффер",
                callbackData: StepKind.DocumentsPreparation.ToString()
            )
        }
    } );
    }
    public static InlineKeyboardMarkup DocumentsPreparation()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Что придет в уведомлениях?",
                callbackData: StepKind.NotificationInfo.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup NotificationInfo()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Понял, давай дальше!",
                callbackData: StepKind.ResolveQuestions.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup ResolveQuestions( string? hrLink )
    {
        if ( string.IsNullOrWhiteSpace( hrLink ) )
        {
            return new InlineKeyboardMarkup( new[]
            {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Все вопросы решены",
                    callbackData: StepKind.PreboardingComplete.ToString()
                )
            }
        } );
        }

        return new InlineKeyboardMarkup( new[]
        {
        new[]
        {
            InlineKeyboardButton.WithUrl(
                text: "📲 Связаться с HR",
                url: hrLink
            )
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(
                text: "Все вопросы решены",
                callbackData: StepKind.PreboardingComplete.ToString()
            )
        }
    } );
    }

    public static InlineKeyboardMarkup PreboardingComplete()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Завершить пребординг",
                callbackData: StepKind.WaitAdminApprove.ToString()
            )
        );
    }
    public static InlineKeyboardMarkup PreboardingFAQ()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "Вернуться к выбору действия",
                callbackData: StepKind.OfferDecision.ToString()
            )
        );
    }
}
