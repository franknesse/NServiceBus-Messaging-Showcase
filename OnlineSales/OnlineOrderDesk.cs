using Microsoft.Extensions.Hosting;
using OnlineSales.Sagas;
using OrderIntakeService.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSales
{
    
    public class OnlineOrderDesk : BackgroundService
    {
        private IMessageSession _messageSession;
        public OnlineOrderDesk(IMessageSession messageSession)
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
                                StartOnlineOrderRequest command = new StartOnlineOrderRequest()
                                {
                                    OrderRequest = new OrderRequest()
                                    {
                                        OrderDetails = "JSON data",
                                        ExternalOrderId = "00003223@rijkzwaan-brazil",
                                        CustomerId = "5525221",
                                        SalesOffice = "RBR"
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
