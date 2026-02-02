namespace TelegramBot.BackgroundServices.Abstract;
public abstract class PollingServiceBase<TReceiverService>( IServiceProvider serviceProvider ) : BackgroundService
    where TReceiverService : IReceiverService
{
    protected override async Task ExecuteAsync( CancellationToken stoppingToken )
    {
        while ( !stoppingToken.IsCancellationRequested )
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                IReceiverService receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();
                await receiver.ReceiveAsync( stoppingToken );
            }
            catch ( Exception ex )
            {
                Console.WriteLine( $"Polling failed with exception: {ex}" );
                await Task.Delay( TimeSpan.FromSeconds( 5 ), stoppingToken );
            }
        }
    }
}