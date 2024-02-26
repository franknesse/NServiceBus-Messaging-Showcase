using OrderIntakeService.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalSales.Sagas
{
    internal class StartInternalOrderRequest : ICommand
    {
        public OrderRequest OrderRequest { get; set; }
    }
}
