using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Core.DataAccess.Abstract;
using Shared.Core.Entities;

namespace Shared.Core.DataAccess.Repositories;

public class MsSqlRepositoryBase<TEntity, TContext> : IMsSqlRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()
{
    private readonly TContext _context;

    public MsSqlRepositoryBase(TContext context)
    {
        _context = context;
    }

    public DbSet<TEntity> Table => _context.Set<TEntity>();

    public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null) =>
        filter == null
            ? Table.AsQueryable()
            : Table.Where(filter).AsQueryable();

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter) =>
        (await Table.SingleOrDefaultAsync(filter))!;

    public virtual async Task<TEntity> GetByIdAsync(string id) =>
        (await Table.FindAsync(id))!;

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        var entityEntry = await Table.AddAsync(entity);
        return entityEntry.State == EntityState.Added;
    }

    public virtual Task<bool> UpdateAsync(TEntity entity, string id = null!)
    {
        var entityEntry = Table.Update(entity);
        return Task.FromResult(entityEntry.State == EntityState.Modified);
    }

    public bool Remove(TEntity entity)
    {
        var entityEntry = Table.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var entity = (await Table.FindAsync(id))!;
        return Remove(entity);
    }

    public async Task<int> SaveAsync() =>
        await _context.SaveChangesAsync();
}