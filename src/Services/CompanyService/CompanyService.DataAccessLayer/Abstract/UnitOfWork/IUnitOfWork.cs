using Microsoft.EntityFrameworkCore;

namespace CompanyService.DataAccessLayer.Abstract.UnitOfWork;

public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
{
    //The following Property is going to hold the context object
    TContext Context { get; }
    
    //Start the database Transaction
    Task CreateTransaction();
    
    //Commit the database Transaction
    Task Commit();
    
    //Rollback the database Transaction
    Task Rollback();

    //DbContext Class SaveChanges method
    Task<int> Save();
}