using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Model
{
    public class RequestedOrder
    {
        public string ExternalOrderId { get; set; } = string.Empty;
        public string SalesOffice { get; set; } = string.Empty;            
        public string CustomerId { get; set; } = string.Empty;
        public string OrderDetails { get; set; }  = string.Empty;
        public bool IsNotified { get; set; } = false;
        public string ErpOrderId { get; set; } = string.Empty;
        public bool IsImported { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsProcessingCompleted { get { return IsImported && IsNotified; } }




    }
}
