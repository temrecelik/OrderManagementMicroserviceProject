using MassTransit;
using MongoDB.Driver;
using Shared.Events.ProductEvents;
using Shared.Events.StockEvents;
using Shared.Messages;
using Stock.EntityLayer.Concrete;

namespace Stock.WebApi.Consumer
{
    public class ProductExistEventConsumer : IConsumer<ProductExistEvent>
    {
        IMongoCollection<EntityLayer.Concrete.Stock> _stockCollection;
        IMongoCollection<ProductInbox> _productInboxCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductExistEventConsumer(IMongoCollection<EntityLayer.Concrete.Stock> productCollection, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, IMongoCollection<ProductInbox> productInboxCollection)
        {
            _stockCollection = productCollection;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _productInboxCollection = productInboxCollection;
        }

        public async Task Consume(ConsumeContext<ProductExistEvent> context)
        {



            List<bool> IsproductExist = new();


            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                IsproductExist.Add((await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any());
                EntityLayer.Concrete.Stock stock = await (await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).FirstOrDefaultAsync();


                if (stock != null)
                {

                    stock.Count -= orderItem.Count;
                    context.Message.totalprice = orderItem.Price * orderItem.Count + context.Message.totalprice;
                    await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);

                }
            }

            if (IsproductExist.TrueForAll(sr => sr.Equals(true)))
            {
                Console.WriteLine("\nTüm ürünler için Stock Yeterli");
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    if (orderItem.Count != 0)
                    {
                        Console.WriteLine(orderItem.ProductId + " ıd'li üründen " + orderItem.Count + "tane alındı. Ürün ücreti: " + orderItem.Count + " x " + orderItem.Price + " TL" + " = " + orderItem.Count * orderItem.Price);
                    }

                }
                Console.WriteLine("Toplam tutar: " + context.Message.totalprice);

                StockReservedEvent stockReservedEvent = new()
                {
                    Success = true,
                    Message = "Stok Yeterli",
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.totalprice,
                    BuyerId = context.Message.BuyerId,

                };

                //ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMqSettings.PaymentStockReservedEventQueue}"));
                //await sendEndpoint.Send(stockReservedEvent);

                await _publishEndpoint.Publish(stockReservedEvent);
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    Success = true,
                    Message = "Stok Yeterli Değil"
                };
                await _publishEndpoint.Publish(stockNotReservedEvent);

                Console.WriteLine("Stock Yeterli Değil");
            }

        }
    }
}