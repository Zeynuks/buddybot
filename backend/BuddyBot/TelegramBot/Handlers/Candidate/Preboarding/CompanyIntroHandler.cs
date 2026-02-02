using AppUser = Domain.Entities.User;

using Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Messages;
using Telegram.Bot.Types.Enums;
using TelegramBot.Services;
using TelegramBot.Extensions;
using TelegramBot.Keyboards.Candidate.Preboarding;
using TelegramBot.Helpers;

namespace TelegramBot.Handlers.Candidate.Preboarding;
public class CompanyIntroHandler( UserService userService, CandidateService candidateService ) : IStepHandler
{
    public StepKind Step => StepKind.CompanyIntro;

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

        string[] introMessages = PreboardingMessages.CompanyIntro;

        await botClient.SendVideoNote(
            chatId: callbackQuery.Message.Chat.Id,
            videoNote: VideoFileIds.AlexeyGerasimovFileId,
            cancellationToken: cancellationToken
        );

        await botClient.SendMessage(
            chatId: callbackQuery.Message.Chat.Id,
            text: PreboardingMessages.CompanyIntroVideoCaption,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );

        for ( int i = 0; i < introMessages.Length; i++ )
        {
            bool isLast = i == introMessages.Length - 1;

            await botClient.SendChatAction(
                chatId: callbackQuery.Message.Chat.Id,
                action: ChatAction.Typing,
                cancellationToken: cancellationToken
            );
            await Task.Delay( TimeSpan.FromSeconds( 4 ), cancellationToken );

            if ( isLast )
            {
                await botClient.SendSticker(
                    chatId: callbackQuery.Message.Chat.Id,
                    sticker: StickerIds.CompanyIntroSticker,
                    cancellationToken: cancellationToken
                );

                await botClient.SendMessage(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: introMessages[ i ],
                    replyMarkup: Inline.CompanyIntro(),
                    parseMode: ParseMode.Html,
                    linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                await botClient.SendMessage(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: introMessages[ i ],
                    parseMode: ParseMode.Html,
                    linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
                    cancellationToken: cancellationToken
                );
            }
        }
    }
}