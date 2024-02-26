using Microsoft.Extensions.Logging;
using OrderIntakeService.Events;
using OrderIntakeService.Messaging.Messages;

namespace OrderProcessingDashboard
{
    public class EventHandler : IHandleMessages<RequestedOrdersModified>,
                                IHandleMessages<GetRequestedOrdersResponse>
    {
        private readonly ILogger log;

        public EventHandler(ILogger<EventHandler> log)
        {
            this.log = log;
        }

        public async Task Handle(RequestedOrdersModified message, IMessageHandlerContext context)
        {
            Console.Clear();                      
            GetRequestedOrders request = new GetRequestedOrders();
            await context.Send(request);
        }

        public async Task Handle(GetRequestedOrdersResponse message, IMessageHandlerContext context)
        {
            Console.ResetColor();
            var defColor = Console.ForegroundColor;

            foreach (var order in message.RequestedOrders)
            {
                if (order.IsImported)
                    if (!order.IsNotified)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                else if (order.HasImportError)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                else
                {
                    Console.ForegroundColor = defColor;
                }
                Console.WriteLine($"{order.SalesOffice}|{order.IsImported}|{order.IsNotified}|{order.IsProcessingCompleted}|{order.CustomerId}|{order.ErpOrderId}|{order.ExternalOrderId}|{order.HasImportError}|{order.Error}");

            }
        }
    }
}
