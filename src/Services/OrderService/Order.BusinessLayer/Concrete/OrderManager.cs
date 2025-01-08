using AutoMapper;
using Order.BusinessLayer.Abstract;
using Order.DataAccessLayer.Abstract;
using Order.EntityLayer.Dtos;
using Shared.Core.CrossCuttingConcerns.Caching;
using Shared.Messages;

namespace Order.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public OrderManager(IOrderDal orderDal, IMapper mapper, ICacheManager cacheManager)
        {
            _orderDal = orderDal;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<EntityLayer.Concrete.Order> AddAsync(CreateOrderDto createOrderDto)
        {
            var entity = _mapper.Map<EntityLayer.Concrete.Order>(createOrderDto);
            await _orderDal.AddAsync(entity);
            await _orderDal.SaveAsync();

            _cacheManager.RemoveByPattern("OrderManager");

            return entity;
        }

        public List<OrderItemMessage> ConvertOrderItemToOrderItemMessage(EntityLayer.Concrete.Order order)
        {
            List<OrderItemMessage> orderItemMessages = new List<OrderItemMessage>();

            foreach (var item in order.OrderItems)
            {
                OrderItemMessage orderItemMessage = new OrderItemMessage
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Count = item.Count
                };

                orderItemMessages.Add(orderItemMessage);
            }

            return orderItemMessages;
        }

        public List<GetOrderDto> GetAll()
        {
            const string cacheKey = "OrderManager.GetAll";
            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<List<GetOrderDto>>(cacheKey);
            }

            List<EntityLayer.Concrete.Order> orders = _orderDal.GetAll().ToList();
            List<GetOrderDto> getOrderDto = _mapper.Map<List<GetOrderDto>>(orders);

            _cacheManager.Add(cacheKey, getOrderDto, duration: 60);

            return getOrderDto;

        }

        public async Task<GetOrderDto> GetByIdAsync(string id)
        {
            string cacheKey = "OrderManager.GetByIdAsync";
            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<GetOrderDto>(cacheKey);
            }

            EntityLayer.Concrete.Order order = await _orderDal.GetByIdAsync(id);
            GetOrderDto getOrderDto = _mapper.Map<GetOrderDto>(order);

            _cacheManager.Add(cacheKey, getOrderDto, duration: 60);

            return getOrderDto;
        }
    }
}