using InternalSales.Sagas;
using OrderIntakeService.Messaging.Messages;

namespace InternalSales.Scheduler
{
    public class Schedular
    {
        private IMessageSession _session;
        private bool _enabled = false;

        public Schedular(IMessageSession session)
        {
            this._session = session;            
        }

        public bool IsRunning { get { return _enabled; } }

        public void Start()
        {
            _enabled = true;
            RunSchedule(_enabled);
        }

        public void Stop()
        {
            _enabled = false;
            RunSchedule(_enabled);
        }

        public void Toggle()
        {
            _enabled = !_enabled;
            RunSchedule(_enabled);
        }


        private async void RunSchedule(bool enabled)
        {
            while (enabled)
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
