using MassTransit;
using MongoDB.Driver;
using ProductService.EntityLayer.Concrete;
using Quartz;
using Shared.Events.ProductEvents;
using System.Text.Json;

namespace ProductService.WebAPI.Jobs
{
    public class ProductOutBoxPublishJob : IJob
    {
        private readonly IMongoCollection<ProductOutbox> _productoutboxCollection;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductOutBoxPublishJob(IMongoCollection<ProductOutbox> productoutboxCollection, IPublishEndpoint publishEndpoint)
        {
            _productoutboxCollection = productoutboxCollection;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("ProductOutBoxPublishJobtetikleniyor");

            List<ProductOutbox> productOutboxes = await _productoutboxCollection
            .Find(o => o.ProcessedDate == null)
            .SortBy(o => o.OccuredON)
            .ToListAsync();

            if (productOutboxes.Any())
            {

                foreach (var productoutbox in productOutboxes)
                {

                    ProductExistEvent productExistEvent = JsonSerializer.Deserialize<ProductExistEvent>(productoutbox.Payload);
                    
                    await _publishEndpoint.Publish(productExistEvent);

                    var filter = Builders<ProductOutbox>.Filter.Eq(o => o.IdempotenToken, productoutbox.IdempotenToken);
                    var update = Builders<ProductOutbox>.Update.Set(o => o.ProcessedDate, DateTime.UtcNow); 

                    await _productoutboxCollection.UpdateOneAsync(filter, update);

                }
            }
        }
    }
}