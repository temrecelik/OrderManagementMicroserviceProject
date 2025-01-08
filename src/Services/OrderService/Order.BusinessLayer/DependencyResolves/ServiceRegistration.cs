using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.BusinessLayer.Abstract;
using Order.BusinessLayer.Concrete;
using Order.DataAccessLayer;
using Order.DataAccessLayer.Abstract;
using Order.DataAccessLayer.Context;
using Shared.Core.CrossCuttingConcerns.Caching.Microsoft;
using Shared.Core.CrossCuttingConcerns.Caching;
using Order.DataAccessLayer.Concrete;

namespace Order.BusinessLayer.DependencyResolves
{
    public static class ServiceRegistration
    {
        public static void AddBusinessService(this IServiceCollection service)
        {
            service.AddDbContext<OrderDbContext>(x => x.UseSqlServer(Configuration.ConnectionString("SqlServer")!));
            service.AddAutoMapper(typeof(ServiceRegistration));

            service.AddScoped<IOrderDal, EfOrderDal>();
            service.AddScoped<IOrderItemDal, EfOrderItemDal>();

            service.AddScoped<IOrderService, OrderManager>();
            service.AddScoped<IOrderItemService,OrderItemManager>();

            service.AddMemoryCache();
            service.AddScoped<ICacheManager, MemoryCacheManager>();

        }
    }
}
