
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.EntityLayer.Concrete
{
    public class OrderItem :IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        Order Order { get; set; }
        public string OrderId { get; set; }

        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; } = 0;
       
    }
}
