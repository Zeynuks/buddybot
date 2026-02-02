using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions;
using TelegramBot.Helpers;
using TelegramBot.Interfaces;
using TelegramBot.Keyboards.Candidate.PersonalArea;
using TelegramBot.Messages;
using TelegramBot.Services;
using Domain.Entities;

namespace TelegramBot.Handlers.Candidate.PersonalArea;
public class PersonalAreaHomeHandler( UserService userService, CandidateService candidateService ) : ICallbackHandler, IMessageHandler
{
    bool IMessageHandler.CanHandle( string messageText )
        => messageText.Trim().ToLower() == "/home";
    bool ICallbackHandler.CanHandle( string callbackData )
        => callbackData == "PersonalAreaHome";

    public async Task HandleAsync( CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        await botClient.RemoveInlineKeyboard( callbackQuery.Message, cancellationToken );

        if ( callbackQuery.Message is null )
        {
            return;
        }

        long telegramId = callbackQuery.From.Id;

        await SendPersonalAreaHome( telegramId, callbackQuery.Message.Chat.Id, botClient, cancellationToken );
    }

    public async Task<bool> HandleAsync( Message message, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        if ( message is null )
        {
            return false;
        }

        long telegramId = message.From?.Id ?? 0;
        if ( telegramId == 0 )
        {
            return false;
        }

        await SendPersonalAreaHome( telegramId, message.Chat.Id, botClient, cancellationToken );
        return true;
    }

    private async Task SendPersonalAreaHome( long telegramId, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken )
    {
        AppUser? candidate = await userService.GetUserByTelegramId( telegramId );
        if ( candidate is null )
        {
            await BotMessageHelper.SendErrorMessage( botClient, chatId, ErrorMessages.CandidateNotFound, cancellationToken );
            return;
        }

        CandidateProcess? activeProcess = await candidateService.GetActiveProcess( candidate.Id );

        if ( activeProcess?.ProcessKind != ProcessKind.PersonalArea )
        {
            await candidateService.TransferToPersonalArea( candidate.Id );
        }

        await botClient.SendSticker(
            chatId: chatId,
            sticker: StickerIds.PersonalAreaHomeSticker,
            cancellationToken: cancellationToken
);
        await botClient.SendMessage(
            chatId: chatId,
            text: PersonalAreaMessages.PersonalAreaHome,
            replyMarkup: Inline.PersonalAreaHome(),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}