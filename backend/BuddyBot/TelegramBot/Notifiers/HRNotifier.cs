using Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Notifiers;
public class HRNotifier( ITelegramBotClient botClient )
{
    public async Task NotifyCandidateHrs(
        User candidate,
        string message,
        CancellationToken cancellationToken,
        ReplyMarkup? replyMarkup = null )
    {
        List<User> hrs = candidate.GetHRs();

        foreach ( User hr in hrs )
        {
            long? hrTelegramId = hr.ContactInfo?.TelegramId;
            if ( hrTelegramId != null )
            {
                await botClient.SendMessage(
                    chatId: hrTelegramId.Value,
                    text: message,
                    parseMode: ParseMode.Html,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken
                );
            }
        }
    }
}