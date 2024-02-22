using InternalSales.Sagas;
using Microsoft.Extensions.Hosting;
using OrderIntakeService.Model.Messages;
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
        public InternalOrderDesk(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Press A to send a message");
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
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
