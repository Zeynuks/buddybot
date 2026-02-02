using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Keyboards.Candidate.Feedback;
public static class Inline
{
    public static InlineKeyboardMarkup PreboardingRating()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData( "⭐☆☆☆☆", "Feedback:Preboarding:1") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐☆☆☆", "Feedback:Preboarding:2") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐☆☆", "Feedback:Preboarding:3") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐⭐☆", "Feedback:Preboarding:4") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐⭐⭐", "Feedback:Preboarding:5") }
        } );
    }


    public static InlineKeyboardMarkup PreboardingComment()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "❌ Нет предложений",
                callbackData: "Feedback:Preboarding:NoSuggestions"
            )
        );
    }

    public static InlineKeyboardMarkup OnboardingRating()
    {
        return new InlineKeyboardMarkup( new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData( "⭐☆☆☆☆", "Feedback:Onboarding:1") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐☆☆☆", "Feedback:Onboarding:2") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐☆☆", "Feedback:Onboarding:3") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐⭐☆", "Feedback:Onboarding:4") },
            new[] { InlineKeyboardButton.WithCallbackData( "⭐⭐⭐⭐⭐", "Feedback:Onboarding:5") }
        } );
    }

    public static InlineKeyboardMarkup OnboardingComment()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "❌ Нет предложений",
                callbackData: "Feedback:Onboarding:NoSuggestions"
            )
        );
    }
}