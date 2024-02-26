using OrderIntakeService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Messaging.Messages
{
    public class GetRequestedOrdersResponse : IMessage
    {
        public IEnumerable<RequestedOrderDto> RequestedOrders { get; set; }    
    }
}
