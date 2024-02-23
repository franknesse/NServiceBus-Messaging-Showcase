using Microsoft.Extensions.Logging;
using NServiceBus.Unicast.Transport;
using OrderIntakeService.Model.Events;

namespace OrderProcessingDashboard
{
    public class EventHandler : IHandleMessages<RequestedOrderCollectionModifiedEvent>
    {
        private readonly ILogger log;

        public EventHandler(ILogger<EventHandler> log)
        {
            this.log = log;
        }

        public async Task Handle(RequestedOrderCollectionModifiedEvent message, IMessageHandlerContext context)
        {
            log.LogInformation(message.Message, context);

            // Sending commands: https://docs.particular.net/nservicebus/messaging/send-a-message#inside-the-incoming-message-processing-pipeline
            // await context.Send(...);

            // Publishing events https://docs.particular.net/nservicebus/messaging/publish-subscribe/publish-handle-event
            // await context.Publish(...);
        }
    }
}
