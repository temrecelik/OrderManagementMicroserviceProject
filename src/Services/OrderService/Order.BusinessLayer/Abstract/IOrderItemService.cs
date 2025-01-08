using Order.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Order.BusinessLayer.Abstract
{
    public interface IOrderItemService
    {
        IQueryable<OrderItem> GetAll();
        Task<OrderItem> GetByIdAsync(string id);
        Task<bool> AddAsync(OrderItem entity);
     
    }
}
