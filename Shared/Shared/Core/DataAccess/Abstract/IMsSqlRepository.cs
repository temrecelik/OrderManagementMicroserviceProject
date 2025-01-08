using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;
using System.Linq.Expressions;

namespace Shared.Core.DataAccess.Abstract;

public interface IMsSqlRepository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity, new()
{
    DbSet<TEntity>? Table { get; }

    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);

    bool Remove(TEntity entity);

    Task<int> SaveAsync();
}