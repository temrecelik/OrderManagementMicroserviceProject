using Order.EntityLayer.Concrete;
using Order.EntityLayer.Dtos;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.BusinessLayer.Abstract
{
    public interface IOrderService
    {
        List<GetOrderDto> GetAll();
        Task<GetOrderDto> GetByIdAsync(string id);
        Task<EntityLayer.Concrete.Order> AddAsync(CreateOrderDto createOrderDto);
        List<OrderItemMessage> ConvertOrderItemToOrderItemMessage(EntityLayer.Concrete.Order order);
    }
}
