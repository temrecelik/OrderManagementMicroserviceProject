using Shared.Core.Entities;

namespace Shared.Core.DataAccess.Abstract;

public interface IMongoRepository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity, new()
{
    Task<List<TEntity>> GetAllAsync();
}