using Microsoft.Extensions.Hosting;
using NServiceBus.Logging;

namespace OrderProcessingDashboard
{
    public class DashboardConsole : BackgroundService
    {      
        
        private static ILog log = LogManager.GetLogger(typeof(DashboardConsole));
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                log.Info("Waiting for events to come in.....");
                while (!cancellationToken.IsCancellationRequested)
                {                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
    }
}
