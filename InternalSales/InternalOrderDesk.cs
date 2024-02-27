using InternalSales.Sagas;
using InternalSales.Scheduler;
using Microsoft.Extensions.Hosting;
using OrderIntakeService.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalSales
{
    internal class InternalOrderDesk : BackgroundService
    {
        private IMessageSession _messageSession;
        private Schedular _schedular;

        public InternalOrderDesk(IMessageSession messageSession, IServiceProvider serviceProvider)
        {
            _messageSession = messageSession;
            _schedular = serviceProvider.GetService(typeof(Schedular)) as Schedular; ;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {            
            try
            {   
                while (!cancellationToken.IsCancellationRequested)
                {                    
                    Console.WriteLine("Press A to submit an order to the OrderIntakeService");
                    Console.WriteLine("Press B to toggle automatic order creation");
                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.A:
                            {
                                Console.WriteLine("Sending OrderRequest message now");
                                StartInternalOrderRequest command = new StartInternalOrderRequest()
                                {
                                    OrderRequest = new OrderRequest()
                                    {
                                        OrderDetails = "JSON data",
                                        ExternalOrderId = "INTORDER 3323221",
                                        CustomerId = "3432423",
                                        SalesOffice = "RDE"
                                    }
                                };
                                await _messageSession.SendLocal(command);
                                break;
                            }
                        case ConsoleKey.B:
                            {
                                Console.WriteLine("Toggle order creation job (for demo)");
                                _schedular.Toggle();
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
