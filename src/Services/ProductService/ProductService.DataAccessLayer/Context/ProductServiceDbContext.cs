using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductService.DataAccessLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core.DataAccess;

namespace ProductService.DataAccessLayer.Context
{
    public class ProductServiceDbContext : MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public ProductServiceDbContext(IOptions<ProductServiceDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public override IMongoCollection<TEntity> GetCollection<TEntity>(string name)
        {
            return _database.GetCollection<TEntity>(name);
        }
    }
}
