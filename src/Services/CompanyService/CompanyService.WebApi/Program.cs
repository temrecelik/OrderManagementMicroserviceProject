using CompanyService.BusinessLayer.DependencyResolvers;
using CompanyService.WebApi.Consumer;
using CompanyService.WebApi.Jobs;
using MassTransit;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Config;
using NLog.Web;
using Quartz;
using Shared;

namespace CompanyService.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            //logger.Debug("Init main");

            var configFilePath = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\Shared\Shared\nlog.config";
            LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);

            var config = LogManager.Configuration;
            config.Variables["logFilePath"] = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\src\Services\CompanyService\CompanyService.WebApi\log\";
            config.Variables["logFileName"] = "companyservice-nlog-${shortdate}";
            LogManager.ReconfigExistingLoggers();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddBusinessService();

                builder.Services.AddQuartz(configurator =>
                {
                    JobKey jobKey = new JobKey("CompanyOutBoxPublishJob");
                    configurator.AddJob<CompanyOutBoxPublishJob>(options => options.WithIdentity(jobKey));

                    TriggerKey triggerKey = new("CompanyOutBoxPublishTrigger");
                    configurator.AddTrigger(options => options.ForJob(jobKey)
                    .WithIdentity(triggerKey)
                    .StartAt(DateTime.UtcNow)
                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5)
                    .RepeatForever()));
                });
                builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                    options.MapType<TimeSpan>(schemaFactory: () => new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("00:00:00")
                    }));

                builder.Services.AddMassTransit(configurator =>
                {
                    configurator.AddConsumer<OrderCreatedEventConsumer>();

                    configurator.UsingRabbitMq(configure: (context, conf) =>
                    {
                        conf.Host(builder.Configuration["RabbitMq"]);

                        conf.ReceiveEndpoint(RabbitMqSettings.CompanyOrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
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
