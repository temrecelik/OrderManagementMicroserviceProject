using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Core.DataAccess;
using Stock.DataAccessLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DataAccessLayer.Context
{
    public class StockDbContext :MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public StockDbContext(IOptions<StockDbSettings> settings)
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
