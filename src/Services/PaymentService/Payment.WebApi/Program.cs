using MassTransit;
using NLog.Config;
using NLog;
using Payment.BusinessLayer.DependencyResolves;
using Payment.WebApi.Consumers;
using Shared;
using NLog.Web;

namespace Payment.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var configFilePath = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\Shared\Shared\nlog.config";
        LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);

        var config = LogManager.Configuration;
        config.Variables["logFilePath"] = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\src\Services\PaymentService\Payment.WebApi\log\";
        config.Variables["logFileName"] = "paymentservice-nlog-${shortdate}";
        LogManager.ReconfigExistingLoggers();

        try
        {

            var builder = WebApplication.CreateBuilder(args);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            // Add services to the container.
            builder.Services.AddBusinessService();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<StockReservedEventConsumer>();

                configurator.UsingRabbitMq((context, _configurator) =>
                {
                    _configurator.Host(builder.Configuration["RabbitMq"]);

                    _configurator.ReceiveEndpoint(RabbitMqSettings.PaymentStockReservedEventQueue, e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
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