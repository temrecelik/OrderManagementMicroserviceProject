using MassTransit;
using Shared;
using Shared.Core.CrossCuttingConcerns.Caching.Microsoft;
using Shared.Core.CrossCuttingConcerns.Caching;
using Stock.BusinessLayer.Absract;
using Stock.BusinessLayer.Concrete;
using Stock.DataAccessLayer.Abstract;
using Stock.DataAccessLayer.Context;
using Stock.DataAccessLayer.EntityFramework;
using Stock.DataAccessLayer.Settings;
using Stock.WebApi.Consumer;
using NLog.Config;
using NLog;
using NLog.Web;
using Stock.EntityLayer.Concrete;

namespace Stock.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configFilePath = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\Shared\Shared\nlog.config";
            LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);

            var config = LogManager.Configuration;
            config.Variables["logFilePath"] = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\src\Services\StockService\Stock.WebApi\log\";
            config.Variables["logFileName"] = "stockservice-nlog-${shortdate}";
            LogManager.ReconfigExistingLoggers();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.Configure<StockDbSettings>(builder.Configuration.GetSection("MongoDB"));
                builder.Services.AddSingleton<StockDbContext>();
                builder.Services.AddSingleton<StockDbSettings>();
                builder.Services.AddControllers();
                builder.Services.AddAutoMapper(typeof(StockManager));
                builder.Services.AddMemoryCache();
                builder.Services.AddScoped<ICacheManager, MemoryCacheManager>();

                builder.Services.AddScoped<IStockDal, EfStockDal>();
                builder.Services.AddScoped<IStockService, StockManager>();
                builder.Services.AddScoped(sp =>
                {
                    var context = sp.GetRequiredService<StockDbContext>();
                    return context.GetCollection<EntityLayer.Concrete.Stock>("Stocks");
                });
                builder.Services.AddScoped(sp =>
                {
                    var context = sp.GetRequiredService<StockDbContext>();
                    return context.GetCollection<ProductInbox>("ProductInboxes");
                });




                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddMassTransit(configurator =>
                {
                    configurator.AddConsumer<ProductExistEventConsumer>();

                    configurator.UsingRabbitMq(configure: (context, conf) =>
                    {
                        conf.Host(builder.Configuration["RabbitMq"]);

                        conf.ReceiveEndpoint(RabbitMqSettings.StockProductExistEventEventQueue, e => e.ConfigureConsumer<ProductExistEventConsumer>(context));
                    });
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
