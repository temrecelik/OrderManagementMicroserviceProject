using Order.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.EntityLayer.Enums;
using Shared.Core.Entities;

namespace Order.EntityLayer.Dtos
{
    public class GetOrderDto : IDto
    {
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; private set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
