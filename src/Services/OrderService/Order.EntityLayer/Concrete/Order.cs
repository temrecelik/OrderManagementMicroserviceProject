
using Order.EntityLayer.Enums;
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Order.EntityLayer.Concrete
{
    public class Order : IEntity
    {
        //public Order(string orderId)
        //{
        //    OrderId = orderId;
        //    CreatedDate = DateTime.Now;
        //}

        //public Order()
        //{
        //    OrderId = Guid.NewGuid().ToString();
        //    CreatedDate = DateTime.Now;
        //}

        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string BuyerId { get; set; }
        public string CompanyId { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Suspend;
        public DateTime CreatedDate { get;  set; } = DateTime.Now;
        
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
