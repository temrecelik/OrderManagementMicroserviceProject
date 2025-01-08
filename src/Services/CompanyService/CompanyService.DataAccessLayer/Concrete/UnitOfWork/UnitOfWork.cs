using CompanyService.DataAccessLayer.Abstract.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyService.DataAccessLayer.Concrete.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
{
    private bool _disposed;
    private IDbContextTransaction _objTran;

    public TContext Context => new();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task CreateTransaction()
    {
        _objTran = await Context.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await _objTran.CommitAsync();
    }

    public async Task Rollback()
    {
        await _objTran.RollbackAsync();
        await _objTran.DisposeAsync();
    }

    public async Task<int> Save()
    {
        return await Context.SaveChangesAsync();
    }
    
    //Context nesnesinin yok edilmesi
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                Context.Dispose();
        _disposed = true;
    }
}