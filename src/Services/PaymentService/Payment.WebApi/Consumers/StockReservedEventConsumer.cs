using MassTransit;
using Shared.Events.PaymentEvents;
using Shared.Events.StockEvents;

namespace Payment.WebApi.Consumers;

public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        var message = context.Message;

        if (message.Success)
        {
            PaymentCompletedEvent paymentCompletedEvent = new()
            {
                OrderId = context.Message.OrderId,
                TotalPrice = context.Message.TotalPrice,
                
            };
            await _publishEndpoint.Publish(paymentCompletedEvent);

            Console.WriteLine("\nÖdeme başarılı\n");
            Console.WriteLine("Sipariş Detayları:\n");
            Console.WriteLine("Buyer Id : " + context.Message.BuyerId);
            Console.WriteLine("Order Id : " + context.Message.OrderId);
            Console.WriteLine("Total Price : " + context.Message.TotalPrice);
        }

        if (!message.Success)
        {
            PaymentFailedEvent paymentFailedEvent = new()
            {
                OrderId = context.Message.OrderId,
                Message = "Bakiye yetersiz..."
                
            };

            await _publishEndpoint.Publish(paymentFailedEvent);

            Console.WriteLine("Ödeme başarısız...");
        }
    }
}