using MassTransit;
using NLog.Config;
using NLog;
using Order.BusinessLayer.DependencyResolves;
using Order.WebApi.Consumer;
using Shared;
using NLog.Web;
using Quartz;
using Order.WebApi.Jobs;

namespace Order.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configFilePath = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\Shared\Shared\nlog.config";
            LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);

            var config = LogManager.Configuration;
            config.Variables["logFilePath"] = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\src\Services\OrderService\Order.WebApi\log\";
            config.Variables["logFileName"] = "orderservice-nlog-${shortdate}";
            LogManager.ReconfigExistingLoggers();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddMassTransit(configurator =>
                {
                    configurator.AddConsumer<PaymentCompletedEventConsumer>();

                    configurator.UsingRabbitMq((context, _configurator) =>
                    {
                        _configurator.Host(builder.Configuration["RabbitMq"]);

                        _configurator.ReceiveEndpoint(RabbitMqSettings.OrderPaymentCompletedEventQueue,
                            e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));
                    });
                });

                builder.Services.AddQuartz(configurator =>
                {
                    JobKey jobKey = new JobKey("OrderOutBoxPublishJob");
                    configurator.AddJob<OrderOutBoxPublishJob>(options => options.WithIdentity(jobKey));

                    TriggerKey triggerKey = new("OrderOutBoxPublishTrigger");
                    configurator.AddTrigger(options => options.ForJob(jobKey)
                    .WithIdentity(triggerKey)
                    .StartAt(DateTime.UtcNow)
                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5)
                    .RepeatForever()));             
                });

                builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
                

                // Add services to the container.
                builder.Services.AddBusinessService();
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

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
