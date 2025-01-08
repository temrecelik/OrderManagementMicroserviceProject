using Shared.Events.Common;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.ProductEvents
{
    public class ProductExistEvent :IEvent
    {
        public ICollection<OrderItemMessage> OrderItems { get; set; }
        public string OrderId { get; set; }
        public string BuyerId { get; set; }
        public string CompanyId { get; set; }
        public decimal totalprice { get; set; }
        public string idempotentToken{ get; set; }
    }
}
