using MassTransit;
using MongoDB.Driver;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DataAccessLayer.Context;
using ProductService.EntityLayer.Concrete;
using Shared.Events.CompanyEvents;
using Shared.Events.ProductEvents;
using Shared.Messages;
using System.Text.Json;

namespace ProductService.WebAPI.Consumer
{
    public class CompanyWorkingHoursSuitableEventConsumer : IConsumer<CompanyWorkingHoursSuitableEvent>
    {
        IMongoCollection<Product> _productCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        private IMongoCollection<CompanyInbox> _inboxCollection;
        private IMongoCollection<ProductOutbox> _productOutboxCollection;

        public CompanyWorkingHoursSuitableEventConsumer(IMongoCollection<Product> productCollection, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, IMongoCollection<CompanyInbox> inboxCollection, IMongoCollection<ProductOutbox> productOutboxCollection)
        {
            _productCollection = productCollection;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _inboxCollection = inboxCollection;
            _productOutboxCollection = productOutboxCollection;
        }

        public async Task Consume(ConsumeContext<CompanyWorkingHoursSuitableEvent> context)
        {
        var idempotenttokenEntity = await _inboxCollection.FindAsync(i => i.IdempotenToken == context.Message.IdempotenToken);
        bool exists = await  idempotenttokenEntity.AnyAsync();

            if (!exists) { 
                CompanyInbox companyInbox = new()
                {
                    IdempotenToken = context.Message.IdempotenToken,
                    Processed = false,
                    Payload = JsonSerializer.Serialize<CompanyWorkingHoursSuitableEvent>(context.Message)

                };
                await _inboxCollection.InsertOneAsync(companyInbox);
                
            }

            var filter = Builders<CompanyInbox>.Filter.Eq(i => i.Processed, false);
            List<CompanyInbox> companyInboxes = await _inboxCollection.Find(filter).ToListAsync();

           foreach (CompanyInbox companyInbox in companyInboxes) {

            CompanyWorkingHoursSuitableEvent companyWorkingHoursSuitableEvent =
                    JsonSerializer.Deserialize<CompanyWorkingHoursSuitableEvent>(companyInbox.Payload);
                   

                  var orderItems = companyWorkingHoursSuitableEvent.OrderItems.ToList();

                List<bool> isProductExist = new();

                foreach (OrderItemMessage orderItem in  companyWorkingHoursSuitableEvent.OrderItems)
                {
                    var a = _productCollection.FindAsync(s => s.ProductId == orderItem.ProductId);
                    isProductExist.Add((await _productCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.CompanyId == context.Message.CompanyId)).Any());

                    if (isProductExist.TrueForAll(sr => sr.Equals(true)))
                    {
                        var product = await _productCollection.FindAsync(_ => _.ProductId == orderItem.ProductId);
                        var productEntity = await product.FirstOrDefaultAsync();
                        orderItem.Price = productEntity.ProductPrice;
                    }

                }

                if (isProductExist.TrueForAll(sr => sr.Equals(true)))
                {

                    Console.WriteLine("\nSeçtiğiniz şirkette eklemek istediğiniz ürün çeşitleri bulunmaktadır.");

                    string idempotentToken = Guid.NewGuid().ToString();

                    ProductExistEvent productExistEvent = new()
                    {

                        OrderItems = orderItems,
                        CompanyId = context.Message.CompanyId,
                        OrderId = context.Message.OrderId,
                        BuyerId = context.Message.BuyerId,
                        totalprice = context.Message.totalprice,
                        idempotentToken = idempotentToken,
                    };

                    ProductOutbox productOutbox = new()
                    {
                        IdempotenToken = idempotentToken,
                        Payload = JsonSerializer.Serialize<ProductExistEvent>(productExistEvent),
                        OccuredON = DateTime.Now,
                        ProcessedDate = null,
                        Type = nameof(ProductExistEvent),
                       
                    };

                    if (productExistEvent != null)
                    {
                        await _productOutboxCollection.InsertOneAsync(productOutbox);
                    }
                    //await _publishEndpoint.Publish(productExistEvent);
                }
                else
                {
                    Console.WriteLine("\nSeçtiğiniz şirkette eklemek istediğiniz ürünler bulunamadı");
                }
            }
        }
    }
   }

