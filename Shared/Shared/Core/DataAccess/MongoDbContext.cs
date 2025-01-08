using MongoDB.Driver;

namespace Shared.Core.DataAccess;

public abstract class MongoDbContext
{
    public abstract IMongoCollection<TEntity> GetCollection<TEntity> (string name);
}