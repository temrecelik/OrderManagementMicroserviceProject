using Shared.Core.Entities;

namespace Shared.Core.DataAccess.Abstract;

public interface IRepository<TEntity> where TEntity : class, IEntity, new()
{
    Task<TEntity> GetByIdAsync(string id);

    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity, string id = null!);
    Task<bool> RemoveAsync(string id);
}