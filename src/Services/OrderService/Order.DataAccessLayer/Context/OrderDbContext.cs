using Microsoft.EntityFrameworkCore;
using Order.EntityLayer.Concrete;

namespace Order.DataAccessLayer.Context
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        { }

        public OrderDbContext()
        { }

        public DbSet<Order.EntityLayer.Concrete.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderOutBox> OrderOutBoxes { get; set; }
    }
}
