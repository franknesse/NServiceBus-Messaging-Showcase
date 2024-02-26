using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using OrderIntakeService.Events;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = configuration.Build();
        Console.Title = config.GetValue<string>("ConsoleTitle");


        var azsbcs = config.GetValue<string>("AzureServiceBusConnectionString");
        if (string.IsNullOrWhiteSpace(azsbcs))
        {
            throw new Exception("Could not read the 'AzureServiceBusConnectionString' variable.");
        }

        await Host.CreateDefaultBuilder()
            .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
            {
                #region create-asb-endpoint-of-bridge
                var orderIntakeServiceBridgeEndpoint = new BridgeEndpoint("OrderIntakeService");
                #endregion

                #region asb-subscribe-to-event-via-bridge
                //asbBridgeEndpoint.RegisterPublisher<MyEvent>("Samples.MessagingBridge.MsmqEndpoint");
                #endregion

                #region asb-bridge-configuration
                var asbBridgeTransport = new BridgeTransport(new AzureServiceBusTransport(azsbcs));
                asbBridgeTransport.AutoCreateQueues = true;
                asbBridgeTransport.HasEndpoint(orderIntakeServiceBridgeEndpoint);
                bridgeConfiguration.AddTransport(asbBridgeTransport);
                #endregion

                #region create-msmq-endpoint-of-bridge
                var dashBoardBridgeEndpoint = new BridgeEndpoint("OrderProcessingDashboard");
                #endregion

                #region msmq-subscribe-to-event-via-bridge
                dashBoardBridgeEndpoint.RegisterPublisher<RequestedOrdersModified>("OrderIntakeService");
                #endregion

                #region msmq-bridge-configuration
                var learningBridgeTransport = new BridgeTransport(new LearningTransport());
                learningBridgeTransport.AutoCreateQueues = true;
                learningBridgeTransport.HasEndpoint(dashBoardBridgeEndpoint);
                bridgeConfiguration.AddTransport(learningBridgeTransport);
                #endregion
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}