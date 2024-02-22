using OrderIntakeService.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSales.Sagas
{
    internal class StartOnlineOrderRequest : ICommand
    {
        public OrderRequest OrderRequest { get; set; }
    }
}
