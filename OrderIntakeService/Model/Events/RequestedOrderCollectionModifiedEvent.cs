using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIntakeService.Model.Events
{
    public class RequestedOrderCollectionModifiedEvent : IEvent
    {
        public string Message = "Hallo";
    }
}
