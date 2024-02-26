using InternalSales.Sagas;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using OrderIntakeService.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternalSales.Scheduler
{
    public class Schedular
    {
        private IMessageSession _session;

        public Schedular(IMessageSession session)
        {
            this._session = session;
        }

        public async void RunSchedule()
        {
            while (true)
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
