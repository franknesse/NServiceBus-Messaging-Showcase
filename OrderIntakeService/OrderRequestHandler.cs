using Microsoft.Extensions.Options;
using NServiceBus.Logging;
using OrderIntakeService.DTO;
using OrderIntakeService.Events;
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
            log.Info($"{DateTime.Now} - Received order request for {message.SalesOffice}/{message.ExternalOrderId}.");
            OrderRequestResponse response = ValidateRequest(message);
            if (response.IsSuccess)
            {
                log.Info($"Order request for {message.SalesOffice}/{message.ExternalOrderId} successfully validated");
                //log.Info($"Order request for {message.SalesOffice}/{message.ExternalOrderId} successfully validated");
                RequestedOrderDto newOrder = new RequestedOrderDto()
                {
                    CustomerId = message.CustomerId,
                    ExternalOrderId = message.ExternalOrderId,
                    OrderDetails = message.OrderDetails,
                    SalesOffice = message.SalesOffice
                };
                _orderRepository.Add(newOrder);
                RequestedOrdersModified ev = new RequestedOrdersModified();
                await context.Publish(ev).ConfigureAwait(false);
            }
            else 
            {
                log.Warn($"Order request for {message.SalesOffice}/{message.ExternalOrderId} cannot be accepted");
                log.Warn($"{response.ErrorInfo}");
            }
            await context.Reply(response).ConfigureAwait(false);            
        }

        private OrderRequestResponse ValidateRequest(OrderRequest message)
        {
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(message.CustomerId))
            {
                sb.AppendLine("Customer Id missing");
            }

            if (String.IsNullOrEmpty(message.SalesOffice))
            {
                sb.AppendLine("SalesOffice missing");
            }

            if (String.IsNullOrEmpty(message.ExternalOrderId))
            {
                sb.AppendLine("ExternalOrderId missing");
            }
            if (sb.ToString() != String.Empty)
            {
                return new OrderRequestResponse()
                {
                    ErrorInfo = sb.ToString(),
                    ExternalOrderId = message.ExternalOrderId,
                    SalesOffice = message.SalesOffice,
                    IsSuccess = false
                };
            }

            RequestedOrderDto existingOrder = _orderRepository.GetOrder(message.SalesOffice, message.ExternalOrderId);
            if (existingOrder != null)
            {
                return new OrderRequestResponse()
                {
                    ErrorInfo = $"Order {message.ExternalOrderId} for salesoffice {message.SalesOffice} already exists",
                    ExternalOrderId = message.ExternalOrderId,
                    SalesOffice = message.SalesOffice,
                    IsSuccess = false
                };
            }
            else
            {
                return new OrderRequestResponse()
                {
                    ErrorInfo = "",
                    ExternalOrderId = message.ExternalOrderId,
                    SalesOffice = message.SalesOffice,
                    IsSuccess = true
                };
            }
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
