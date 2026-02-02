namespace TelegramBot.BackgroundServices.Abstract;
public interface IReceiverService
{
    Task ReceiveAsync( CancellationToken stoppingToken );
}