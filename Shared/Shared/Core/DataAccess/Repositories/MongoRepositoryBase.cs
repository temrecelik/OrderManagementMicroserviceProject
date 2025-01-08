using MongoDB.Driver;
using Shared.Core.DataAccess.Abstract;
using Shared.Core.Entities;

namespace Shared.Core.DataAccess.Repositories;

public class MongoRepositoryBase<TEntity, TContext> : IMongoRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : MongoDbContext
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepositoryBase(TContext context, string collectionName)
    {
        _collection = context.GetCollection<TEntity>(collectionName);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
 
    public virtual async Task<TEntity> GetByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return true;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity, string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
        return true;
    }

    public virtual async Task<bool> RemoveAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
        return true;
    }
}