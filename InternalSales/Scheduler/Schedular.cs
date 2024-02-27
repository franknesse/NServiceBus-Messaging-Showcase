using InternalSales.Sagas;
using NServiceBus.Logging;
using OrderIntakeService.Messaging.Messages;

namespace InternalSales.Scheduler
{
    public class Schedular
    {
        private IMessageSession _session;
        private bool _enabled = false;
        private static ILog _log;

        public Schedular(IMessageSession session)
        {
            this._session = session;            
            _log = LogManager.GetLogger<Schedular>();
        }

        public bool IsRunning { get { return _enabled; } }

        public void Start()
        {
            RunSchedule(true);
        }

        public void Stop()
        {
            RunSchedule(false);
        }

        public void Toggle()
        {
            bool enabled = !IsRunning;
            RunSchedule(enabled);
        }


        private async void RunSchedule(bool enabled)
        {
            _enabled = enabled;
            if (enabled)
            {
                _log.Info("Enabling automatic order creation");
            }
            else
            {
                _log.Info("Disabling automatic order creation");
            }
            while (_enabled)
            {
                Random rnd = new Random();
                int sleep = rnd.Next(1000, 10000);
                Console.WriteLine("Sending OrderRequest message now");
                StartInternalOrderRequest command = new StartInternalOrderRequest()
                {
                    OrderRequest = new OrderRequest()
                    {
                        OrderDetails = "<JSON data>",
                        ExternalOrderId = $"INTERNAL ORDER {Guid.NewGuid().ToString().Substring(0,5).ToUpper()}",
                        CustomerId = sleep.ToString(),
                        SalesOffice = "RDE"
                    }
                };
                await _session.SendLocal(command);
                
                Thread.Sleep(sleep);
            }
        }
    }
}
