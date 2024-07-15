using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;

namespace BackBroundFix_WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly WatcherService watcherService;
        private readonly ILogger<Worker> logger;

        public Worker(WatcherService awatcherService, ILogger<Worker> alogger)
        {
            this.watcherService = awatcherService;
            this.logger = alogger;

            watcherService.DisplayCountChanged += WhenDisplayCountChange;

            WhenDisplayCountChange(this, watcherService.displayCount);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    if (logger.IsEnabled(LogLevel.Information))
            //    {
            //        string message = watcherService.getMessage();
            //        logger.LogWarning(message);
            //    }
            //    await Task.Delay(10000, stoppingToken);
            //}
        }

        void WhenDisplayCountChange(object sender, int newDisplayCount)
        {
            logger.LogInformation($"Change in number of display: {newDisplayCount}");

            var builder = new ToastContentBuilder()
                .AddArgument("conversationId", 9813)
                .AddText("Change in connected displayes")
                .AddText($"New number of displayes: {watcherService.displayCount}");
            builder.Show(toast => { toast.ExpirationTime = DateTime.Now.AddSeconds(60); });
        }

        public override void Dispose()
        {
            watcherService.DisplayCountChanged -= WhenDisplayCountChange;
            watcherService.Dispose();
            base.Dispose();
        }
    }
}
