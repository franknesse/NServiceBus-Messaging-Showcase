using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Messaging.Messages
{
    public class OrderRequestResponse : IMessage
    {
        public string ExternalOrderId { get; set; }        
        public bool IsSuccess { get; set; }
        public string ErrorInfo { get; set; }
    }
}
