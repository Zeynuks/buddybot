using Telegram.Bot;
using TelegramBot.BackgroundServices.Abstract;

namespace TelegramBot.BackgroundServices.Services;
public class ReceiverService( ITelegramBotClient botClient, UpdateHandler updateHandler ) 
    : ReceiverServiceBase<UpdateHandler>( botClient, updateHandler )
{
}