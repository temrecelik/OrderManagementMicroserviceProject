using CompanyService.BusinessLayer.Abstract;
using CompanyService.BusinessLayer.Concrete;
using CompanyService.DataAccessLayer;
using CompanyService.DataAccessLayer.Abstract;
using CompanyService.DataAccessLayer.Concrete;
using CompanyService.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.CrossCuttingConcerns.Caching.Microsoft;
using Shared.Core.CrossCuttingConcerns.Caching;

namespace CompanyService.BusinessLayer.DependencyResolvers;

public static class ServiceRegistration
{
    public static void AddBusinessService(this IServiceCollection services)
    {
        services.AddDbContext<CompanyDbContext>(x => x.UseSqlServer(Configuration.ConnectionString(connectionName:"SqlServer")!));
        services.AddAutoMapper(typeof(ServiceRegistration));

        services.AddScoped<ICompanyDal, CompanyDal>();

        services.AddScoped<ICompanyService, CompanyManager>();

        services.AddMemoryCache();
        services.AddScoped<ICacheManager, MemoryCacheManager>();

    }
}