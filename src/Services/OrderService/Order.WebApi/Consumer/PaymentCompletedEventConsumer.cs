using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.DataAccessLayer.Context;
using Order.EntityLayer.Enums;
using Shared.Events.PaymentEvents;

namespace Order.WebApi.Consumer;

public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
{
    private readonly OrderDbContext _context;

    public PaymentCompletedEventConsumer(OrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        EntityLayer.Concrete.Order order =  await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);

        if (order != null )
        {
            order!.OrderStatus = OrderStatus.Completed;
            order.TotalPrice = context.Message.TotalPrice;  
            await _context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Hata");
        }
    }
}