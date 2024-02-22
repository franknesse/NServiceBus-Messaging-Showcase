using NServiceBus.Logging;
using OrderIntakeService.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSales.Sagas
{
    internal class OrderRequestPolicy : Saga<OrderRequestData>,
                                        IAmStartedByMessages<StartOnlineOrderRequest>,
                                        IHandleMessages<OrderRequestResponse>
    {
        private static ILog log = LogManager.GetLogger(typeof(OrderRequestPolicy));
        public async Task Handle(StartOnlineOrderRequest command, IMessageHandlerContext context)
        {
            this.Data.OrderId = command.OrderRequest.ExternalOrderId;             
            await context.Send(command.OrderRequest);
        }

        public Task Handle(OrderRequestResponse message, IMessageHandlerContext context)
        {
            log.Info(message.ExternalOrderId);
            return Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderRequestData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.OrderId)
        .ToMessage<StartOnlineOrderRequest>(message => $"{message.OrderRequest.ExternalOrderId}");
            mapper.MapSaga(sagaData => sagaData.OrderId)
        .ToMessage<OrderRequestResponse>(message => $"{message.ExternalOrderId}");
        }
    }


    internal class OrderRequestData : ContainSagaData
    {
        public string OrderId { get; set; }
    }
}
