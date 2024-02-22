using NServiceBus.Logging;
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
        static ILog log = LogManager.GetLogger(typeof(MessageHandler));
        public async Task Handle(OrderRequest message, IMessageHandlerContext context)
        {
            log.Info($"{DateTime.Now} - Received order request for {message.ExternalOrderId}.");
            var response = new OrderRequestResponse()
            {
                ExternalOrderId = message.ExternalOrderId,
                ErrorInfo = "",
                IsSuccess = true
            };
            await context.Reply(response)
            .ConfigureAwait(false);
        }
    }
}
