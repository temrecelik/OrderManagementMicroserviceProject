using Order.EntityLayer.Concrete;
using Order.EntityLayer.Enums;
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Order.EntityLayer.Dtos
{
    public class CreateOrderDto : IDto
    {
      
        public string BuyerId { get; set; }
        public string CompanyId { get; set; }
        public ICollection<CreateOrderItemDto> OrderItems { get; set; }

    }
}
