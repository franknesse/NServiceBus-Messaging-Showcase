using NServiceBus.Logging;
using OrderIntakeService.DTO;
using OrderIntakeService.Messaging.Events;
using OrderIntakeService.Messaging.Messages;
using OrderIntakeService.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService
{
    public class OrderRequestHandler : IHandleMessages<OrderRequest>,
                                        IHandleMessages<GetRequestedOrders>
    {
        private OrderRepository _orderRepository;
        public OrderRequestHandler(OrderRepository repository)
        {
            _orderRepository = repository;
        }
        static ILog log = LogManager.GetLogger(typeof(OrderRequestHandler));
        public async Task Handle(OrderRequest message, IMessageHandlerContext context)
        {
            log.Info($"{DateTime.Now} - Received order request for {message.ExternalOrderId}.");
            RequestedOrderDto newOrder = new RequestedOrderDto()
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
            RequestedOrderCollectionModifiedEvent ev = new RequestedOrderCollectionModifiedEvent();            
            await context.Publish(ev).ConfigureAwait(false);
        }       

        public async Task Handle(GetRequestedOrders message, IMessageHandlerContext context)
        {
            GetRequestedOrdersResponse resp = new GetRequestedOrdersResponse()
            {
                RequestedOrders = _orderRepository.RequestedOrders
            };
            await context.Reply(resp).ConfigureAwait(false);
        }
    }
}
