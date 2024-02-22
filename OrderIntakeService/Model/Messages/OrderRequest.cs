using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Model.Messages
{
    public class OrderRequest : IMessage
    {   
        public string ExternalOrderId { get; set; }
        public string SalesOffice { get; set; }
        public string CustomerId { get; set; }
        public string OrderDetails { get; set; }
    }
}
