using NServiceBus.Logging;
using OrderIntakeService.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalSales.Sagas
{
    internal class InternalOrderRequestPolicy : Saga<InternalOrderRequestData>, 
                                        IAmStartedByMessages<StartInternalOrderRequest>,
                                        IHandleMessages<OrderRequestResponse>
    {
        private static ILog log = LogManager.GetLogger(typeof(InternalOrderRequestPolicy));
        public async Task Handle(StartInternalOrderRequest command, IMessageHandlerContext context)
        {
            this.Data.OrderId = command.OrderRequest.ExternalOrderId;
            this.Data.CustomerId = command.OrderRequest.CustomerId;
            log.Info($"{DateTime.Now} - Starting new InternalOrderRequest: CustomerId={this.Data.CustomerId}, OrderId={command.OrderRequest.ExternalOrderId}");
            await context.Send(command.OrderRequest);
        }

        public Task Handle(OrderRequestResponse message, IMessageHandlerContext context)
        {
            log.Info($"Handling response for RequestId={message.ExternalOrderId}");
            return Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<InternalOrderRequestData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.OrderId)
        .ToMessage<StartInternalOrderRequest>(message => $"{message.OrderRequest.ExternalOrderId}");
            mapper.MapSaga(sagaData => sagaData.OrderId)
        .ToMessage<OrderRequestResponse>(message => $"{message.ExternalOrderId}");
        }
    }


    internal class InternalOrderRequestData : ContainSagaData
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }
}
