using MassTransit;
using ProductService.BusinessLayer.Abstract;
using ProductService.BusinessLayer.Concrete;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DataAccessLayer.Context;
using ProductService.DataAccessLayer.EntityFramework;
using ProductService.DataAccessLayer.Settings;
using ProductService.EntityLayer.Concrete;
using ProductService.WebAPI.Consumer;
using Shared;
using Shared.Core.CrossCuttingConcerns.Caching.Microsoft;
using Shared.Core.CrossCuttingConcerns.Caching;
using NLog.Config;
using NLog;
using NLog.Web;
using Quartz;
using ProductService.WebAPI.Jobs;

var configFilePath = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\Shared\Shared\nlog.config";
LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);

var config = LogManager.Configuration;
config.Variables["logFilePath"] = @"C:\Users\PC\Desktop\.Net-Projects\InternProjectAlphaPhase2\OrderManagementMicroservice\src\Services\ProductService\ProductService.WebAPI\log\";
config.Variables["logFileName"] = "productservice-nlog-${shortdate}";
LogManager.ReconfigExistingLoggers();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.Configure<ProductServiceDbSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddSingleton<ProductServiceDbContext>();
    builder.Services.AddSingleton<ProductServiceDbSettings>();
    builder.Services.AddScoped(sp =>
    {
        var context = sp.GetRequiredService<ProductServiceDbContext>();
        return context.GetCollection<Product>("Products");
    });
    builder.Services.AddScoped(sp =>
    {
        var context = sp.GetRequiredService<ProductServiceDbContext>();
        return context.GetCollection<CompanyInbox>("CompanyInBoxes");
    });
    builder.Services.AddScoped(sp =>
    {
        var context = sp.GetRequiredService<ProductServiceDbContext>();
        return context.GetCollection<ProductOutbox>("ProductOutboxes");
    });

    builder.Services.AddQuartz(configurator =>
    {
        JobKey jobKey = new JobKey("ProductOutBoxPublishJob");
        configurator.AddJob<ProductOutBoxPublishJob>(options => options.WithIdentity(jobKey));

        TriggerKey triggerKey = new("ProductOutBoxPublishTrigger");
        configurator.AddTrigger(options => options.ForJob(jobKey)
        .WithIdentity(triggerKey)
        .StartAt(DateTime.UtcNow)
        .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5)
        .RepeatForever()));
    });
    builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);



    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ICacheManager, MemoryCacheManager>();

    builder.Services.AddControllers();
    builder.Services.AddScoped<IProductDal, EfProductDal>();
    builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

    builder.Services.AddScoped<IProductService, ProductManager>();
    builder.Services.AddScoped<ICategoryService, CategoryManager>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddMassTransit(configurator =>
    {
        configurator.AddConsumer<CompanyWorkingHoursSuitableEventConsumer>();

        configurator.UsingRabbitMq(configure: (context, conf) =>
        {
            conf.Host(builder.Configuration["RabbitMq"]);

            conf.ReceiveEndpoint(RabbitMqSettings.ProductCompanyWorkingHoursSuitableEventQueue,
                e => e.ConfigureConsumer<CompanyWorkingHoursSuitableEventConsumer>(context));
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