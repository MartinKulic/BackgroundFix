using System.Windows.Forms;

namespace BackBroundFix_WorkerService
{
    public class Worker(WatcherService watcherService, ILogger<Worker> logger) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    string message = watcherService.getMessage();
                    logger.LogWarning(message);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
