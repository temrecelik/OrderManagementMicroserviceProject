using CompanyService.DataAccessLayer.Context;
using CompanyService.EntityLayer.Concrete;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events.CompanyEvents;
using Shared.Events.OrderEvents;
using System.Text.Json;

namespace CompanyService.WebApi.Consumer;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly CompanyDbContext _context;

    public OrderCreatedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, CompanyDbContext context)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _publishEndpoint = publishEndpoint;
        _context = context;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var IdempotentKey = await _context.OrderInboxes.AnyAsync(i => i.IdempotenToken == context.Message.IdempotenToken);

        if (!IdempotentKey) 
        { 

        await  _context.OrderInboxes.AddAsync(new()
        {
            IdempotenToken = context.Message.IdempotenToken,
            Processed = false,
            Payload = JsonSerializer.Serialize<OrderCreatedEvent>(context.Message)
        });

        await _context.SaveChangesAsync();
        }

        List<OrderInbox> orderInboxes = await _context.OrderInboxes
            .Where(i => i.Processed == false)
            .ToListAsync();


        foreach (var orderInbox in orderInboxes)
        {
            OrderCreatedEvent orderCreatedEvent =  JsonSerializer.Deserialize<OrderCreatedEvent>(orderInbox.Payload);   

            //var company = await _context.Companies.FindAsync(context.Message.CompanyId);
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id ==  orderCreatedEvent.CompanyId);
            var orderItems = orderCreatedEvent.OrderItemMessages.ToList();

            TimeSpan now = DateTime.Now.TimeOfDay;

            if (company == null)
            {
                Console.WriteLine("Şirket Yok");
            }

            else if (company!.OpeningTime <= now && company.ClosingTime > now)
            {
                Console.WriteLine("Çalışma saatleri içerisinde");

                string IdempotenToken = Guid.NewGuid().ToString();
                CompanyWorkingHoursSuitableEvent companyWorkingHoursSuitableEvent = new()
                {
                    Message = "Çalışma saatleri içerisinde",
                    OrderItems = orderItems,
                    CompanyId = orderCreatedEvent.CompanyId,
                    BuyerId = orderCreatedEvent.BuyerId,
                    OrderId = orderCreatedEvent.OrderId,
                    IdempotenToken = IdempotenToken

                };

                CompanyOutbox companyOutbox = new()
                {
                    IdempotenToken = IdempotenToken,
                    OccuredON = DateTime.Now,
                    ProcessedDate = null,
                    Type = nameof(companyWorkingHoursSuitableEvent),
                    Payload = JsonSerializer.Serialize(companyWorkingHoursSuitableEvent)

                };
               await _context.CompanyOutboxes.AddAsync(companyOutbox);
               await _context.SaveChangesAsync();


                //ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(RabbitMqSettings.StockCompanyWorkingHoursSuitableEventQueue));
                //await sendEndpoint.Send(companyWorkingHoursSuitableEvent);

                //await _publishEndpoint.Publish(companyWorkingHoursSuitableEvent);
            }
            else
            {
                Console.WriteLine("Çalışma saatleri dışında");
            }

            orderInbox.Processed = true;
            await _context.SaveChangesAsync();
        }
    }

}