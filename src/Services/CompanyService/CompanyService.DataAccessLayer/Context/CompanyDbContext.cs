using CompanyService.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.DataAccessLayer.Context;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions options) : base(options)
    { }

    public CompanyDbContext()
    { }

    public DbSet<EntityLayer.Concrete.Company> Companies { get; set; }
    public DbSet<OrderInbox> OrderInboxes { get; set; } 
    public DbSet<CompanyOutbox> CompanyOutboxes { get; set; }
  
}
