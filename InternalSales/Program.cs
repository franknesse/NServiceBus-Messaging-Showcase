using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderIntakeService.Model.Messages;

namespace InternalSales
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Internal Sales";
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseNServiceBus(ctx =>
                {
                    // TODO: consider moving common endpoint configuration into a shared project
                    // for use by all endpoints in the system

                    var endpointConfiguration = new EndpointConfiguration("InternalSales");

                    // Learning Transport: https://docs.particular.net/transports/learning/
                    var routing = endpointConfiguration.UseTransport(new LearningTransport());
                    routing.RouteToEndpoint(typeof(OrderRequest), "OrderIntakeService");

                    // Define routing for commands: https://docs.particular.net/nservicebus/messaging/routing#command-routing
                    // routing.RouteToEndpoint(typeof(MessageType), "DestinationEndpointForType");
                    // routing.RouteToEndpoint(typeof(MessageType).Assembly, "DestinationForAllCommandsInAsembly");

                    // Learning Persistence: https://docs.particular.net/persistence/learning/
                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    // Message serialization
                    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                    // Installers are useful in development. Consider disabling in production.
                    // https://docs.particular.net/nservicebus/operations/installers
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .ConfigureServices(c => c.AddHostedService<InternalOrderDesk>());
        }

        static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
        {
            // TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
            // https://docs.particular.net/nservicebus/hosting/critical-errors
            // and consider setting up service recovery
            // https://docs.particular.net/nservicebus/hosting/windows-service#installation-restart-recovery
            try
            {
                await context.Stop(cancellationToken);
            }
            finally
            {
                FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }

        static void FailFast(string message, Exception exception)
        {
            try
            {
                // TODO: decide what kind of last resort logging is necessary
                // TODO: when using an external logging framework it is important to flush any pending entries prior to calling FailFast
                // https://docs.particular.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }
    }
}
