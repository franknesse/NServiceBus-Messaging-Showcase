using InternalSales.Sagas;
using InternalSales.Scheduler;
using Microsoft.Extensions.Hosting;
using OrderIntakeService.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalSales
{
    internal class InternalOrderDesk : BackgroundService
    {
        private IMessageSession _messageSession;
        private Schedular _schedular;

        public InternalOrderDesk(IMessageSession messageSession, IServiceProvider serviceProvider)
        {
            _messageSession = messageSession;
            _schedular = serviceProvider.GetService(typeof(Schedular)) as Schedular; ;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {            
            try
            {                
                _schedular.RunSchedule();
                while (!cancellationToken.IsCancellationRequested)
                {                    
                    Console.WriteLine("Press A to send a message");
                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.A:
                            {
                                Console.WriteLine("Sending OrderRequest message now");
                                StartInternalOrderRequest command = new StartInternalOrderRequest()
                                {
                                    OrderRequest = new OrderRequest()
                                    {
                                        OrderDetails = "JSON data",
                                        ExternalOrderId = "INTORDER 3323221",
                                        CustomerId = "3432423",
                                        SalesOffice = "RDE"
                                    }
                                };
                                await _messageSession.SendLocal(command);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void StartSchedules()
        {
            await _messageSession.ScheduleEvery(
        timeSpan: TimeSpan.FromSeconds(5),
        task: pipelineContext =>
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                CustomerId = Guid.NewGuid().ToString().Substring(0, 5),
                ExternalOrderId = Guid.NewGuid().ToString().Substring(0, 5),
                OrderDetails = String.Empty,
                SalesOffice = "RBR"
            };
            var message = new StartInternalOrderRequest()
            {
                OrderRequest = orderRequest
            };
            return pipelineContext.SendLocal(message);
        });
        }

    }
}
