using TelegramBot.BackgroundServices.Abstract;

namespace TelegramBot.BackgroundServices.Services;

public class PollingService( IServiceProvider serviceProvider) : 
    PollingServiceBase<ReceiverService>( serviceProvider )
{
}
