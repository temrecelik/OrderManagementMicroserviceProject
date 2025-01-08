using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.BusinessLayer.Abstract;
using Order.DataAccessLayer.Context;
using Order.EntityLayer.Concrete;
using Order.EntityLayer.Dtos;
using Shared.Events.OrderEvents;
using Shared.Messages;
using System.Text.Json;

namespace Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderControlller : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderControlller> _logger;
        private readonly OrderDbContext _orderDbContext;

        public OrderControlller(IOrderService orderService, IPublishEndpoint publishEndpoint, ILogger<OrderControlller> logger, OrderDbContext orderDbContext)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _orderDbContext = orderDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("OrderLog");
            var result = _orderService.GetAll();
            return Ok(result);
        }

        [HttpGet(template: "id")]
        public IActionResult Get(string id)
        {
            var result = _orderService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateOrderDto createOrderDto)
        {
            var Order = await _orderService.AddAsync(createOrderDto);
            List<OrderItemMessage> orderItemMessages = _orderService.ConvertOrderItemToOrderItemMessage(Order);

            string IdempotenToken = Guid.NewGuid().ToString();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                CompanyId = createOrderDto.CompanyId,
                BuyerId = createOrderDto.BuyerId,
                totalprice = Order.TotalPrice,
                OrderId = Order.OrderId,
                IdempotenToken = IdempotenToken,
                OrderItemMessages = orderItemMessages

            };

            OrderOutBox orderOutBox = new()
            {
                IdempotenToken = IdempotenToken,
                OccuredON = DateTime.Now,
                ProcessedDate = null,
                Type = nameof(orderCreatedEvent),
                Payload = JsonSerializer.Serialize(orderCreatedEvent),
            };

            await _orderDbContext.OrderOutBoxes.AddAsync(orderOutBox);
            await _orderDbContext.SaveChangesAsync();

            //await _publishEndpoint.Publish(orderCreatedEvent);
            
            return Ok();
        }
    }
}
