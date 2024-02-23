using NServiceBus.Logging;
using OrderIntakeService.Model;
using OrderIntakeService.Model.Events;
using OrderIntakeService.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService
{
    public class MessageHandler : IHandleMessages<OrderRequest>
    {
        private OrderRepository _orderRepository;
        public MessageHandler(OrderRepository repository) 
        {
            _orderRepository = repository;
        } 
        static ILog log = LogManager.GetLogger(typeof(MessageHandler));
        public async Task Handle(OrderRequest message, IMessageHandlerContext context)
        {
            log.Info($"{DateTime.Now} - Received order request for {message.ExternalOrderId}.");
            RequestedOrder newOrder = new RequestedOrder()
            {
                CustomerId = message.CustomerId,
                ExternalOrderId = message.ExternalOrderId,
                OrderDetails = message.OrderDetails,
                SalesOffice = message.SalesOffice
            };
            _orderRepository.Add(newOrder);            

            var response = new OrderRequestResponse()
            {
                ExternalOrderId = message.ExternalOrderId,
                ErrorInfo = "",
                IsSuccess = true
            };
            await context.Reply(response).ConfigureAwait(false);
            await context.Publish(new RequestedOrderCollectionModifiedEvent()).ConfigureAwait(false);
        }
    }
}
