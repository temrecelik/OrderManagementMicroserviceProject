using Microsoft.Extensions.DependencyInjection;
using Payment.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Payment.DataAccessLayer;

namespace Payment.BusinessLayer.DependencyResolves
{
    public static class ServiceRegistration
    {
        public static void AddBusinessService(this IServiceCollection service)
        {
            service.AddDbContext<PaymentDbContext>(x =>
            {
                x.UseSqlServer(Configuration.ConnectionString("SqlServer"));
            });
        }
    }
}
