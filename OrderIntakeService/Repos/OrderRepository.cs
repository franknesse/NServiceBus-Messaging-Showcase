using OrderIntakeService.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Repos
{
    public class OrderRepository
    {
        private List<RequestedOrderDto> requestedOrders = new List<RequestedOrderDto>();
        public IEnumerable<RequestedOrderDto> RequestedOrders
        { get { return requestedOrders.AsEnumerable<RequestedOrderDto>() ; } }

        public void Add(RequestedOrderDto requestedOrder)
        {
            requestedOrders.Add(requestedOrder);
        }

        internal RequestedOrderDto GetOrder(string salesOffice, string externalOrderId)
        {
            return requestedOrders.FirstOrDefault(x => x.SalesOffice == salesOffice && x.ExternalOrderId == externalOrderId); 
        }
    }
}
