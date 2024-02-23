using OrderIntakeService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService
{
    public class OrderRepository
    {
        private List<RequestedOrder> requestedOrders = new List<RequestedOrder>();
        public ReadOnlyCollection<RequestedOrder> RequestedOrders
            { get { return requestedOrders.AsReadOnly<RequestedOrder>(); } }

        public void Add(RequestedOrder requestedOrder)
        {
            requestedOrders.Add(requestedOrder);
        }
    }
}
