using Microsoft.Extensions.Hosting;
using OrderIntakeService.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Services
{
    internal class ProcessSchedule : BackgroundService
    {
        private OrderRepository _orderRepository;

        public ProcessSchedule(IServiceProvider serviceProvider)
        {
            this._orderRepository = (OrderRepository)serviceProvider.GetService(typeof(OrderRepository));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var orders = _orderRepository.RequestedOrders.Where(o => o.IsImported && !o.IsProcessingCompleted);
                if (orders != null)
                {
                    foreach (var o in orders)
                    {
                        o.IsNotified = true;
                    }
                }
                orders = _orderRepository.RequestedOrders.Where(o => !o.IsImported);
                if (orders != null)
                {
                    foreach (var o in orders)
                    {
                        if (o.ExternalOrderId.EndsWith("A"))
                        {
                            o.Error = "IMPORT FAILED";
                            o.HasImportError = true;
                        }
                        else
                        {
                            o.IsImported = true;
                            o.ErpOrderId = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                        }
                    }
                }
                Thread.Sleep(20000);
            }
        }

        
    }
}
