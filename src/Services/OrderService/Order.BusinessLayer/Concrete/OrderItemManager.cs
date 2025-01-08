using Order.BusinessLayer.Abstract;
using Order.DataAccessLayer.Abstract;
using Order.EntityLayer.Concrete;

namespace Order.BusinessLayer.Concrete
{
    public class OrderItemManager :IOrderItemService
    {
        private readonly IOrderDal _orderDal;

        public OrderItemManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public Task<bool> AddAsync(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OrderItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
