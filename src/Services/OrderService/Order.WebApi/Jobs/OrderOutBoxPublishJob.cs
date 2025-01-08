using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.DataAccessLayer.Context;
using Order.EntityLayer.Concrete;
using Quartz;
using Shared.Events.OrderEvents;
using System.Text.Json;

namespace Order.WebApi.Jobs
{
    public class OrderOutBoxPublishJob : IJob
    {
        IPublishEndpoint _publishEndpoint;
        OrderDbContext _orderDbContext;

        public OrderOutBoxPublishJob(IPublishEndpoint publishEndpoint, OrderDbContext orderDbContext)
        {
            _publishEndpoint = publishEndpoint;
            _orderDbContext = orderDbContext;
        }

        public  async Task Execute(IJobExecutionContext context)
         {
            List<OrderOutBox> orderOutBoxes = await _orderDbContext.OrderOutBoxes
            .Where(o => o.ProcessedDate == null)
            .OrderBy(o => o.OccuredON)
            .ToListAsync();
           

            foreach (var orderOutBox in orderOutBoxes)
            {     
                    OrderCreatedEvent orderCreatedEvent =
                        JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutBox.Payload);
                  

                    if (orderCreatedEvent != null) {
                        
                        await _publishEndpoint.Publish(orderCreatedEvent);
                        orderOutBox.ProcessedDate = DateTime.Now;
                        await _orderDbContext.SaveChangesAsync();
                    } 

                
            }
            Console.WriteLine("Order Outbox Tetiklendi");

        }    
    }
}
