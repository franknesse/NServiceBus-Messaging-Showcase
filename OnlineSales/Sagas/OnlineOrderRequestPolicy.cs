using NServiceBus.Logging;
using OrderIntakeService.Messaging.Messages;
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Handling OrderRequestResponse from OrderIntakeService");            
            sb.AppendLine($"SalesOffice: {message.SalesOffice}");
            sb.AppendLine($"OrderId    : {message.ExternalOrderId}");
            sb.AppendLine($"Success?   : {message.IsSuccess}");
            
            if (!message.IsSuccess)
            {
                sb.AppendLine($"Error      : {message.ErrorInfo}");                
                this.MarkAsComplete();
                sb.AppendLine("OrderRequestSaga marked as completed");
                log.Warn(sb.ToString());
            }
            else
            {
                log.Info(sb.ToString());
            }
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
