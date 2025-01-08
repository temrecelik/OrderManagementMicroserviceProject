using CompanyService.DataAccessLayer.Context;
using CompanyService.EntityLayer.Concrete;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shared.Events.CompanyEvents;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CompanyService.WebApi.Jobs
{
    public class CompanyOutBoxPublishJob : IJob
    {
       private readonly IPublishEndpoint _publishEndpoint;
        private readonly CompanyDbContext _companyDbContext;

        public CompanyOutBoxPublishJob(IPublishEndpoint publishEndpoint, CompanyDbContext companyDbContext)
        {
            _publishEndpoint = publishEndpoint;
            _companyDbContext = companyDbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("companyjob tetiklenşiyor");

            List<CompanyOutbox> companyOutboxes = await _companyDbContext.CompanyOutboxes
                .Where(o => o.ProcessedDate == null)
                .OrderBy(o => o.OccuredON)
                .ToListAsync();

            foreach (var companyOutbox in companyOutboxes)
            {

                CompanyWorkingHoursSuitableEvent companyWorkingHoursSuitableEvent =
                    JsonSerializer.Deserialize<CompanyWorkingHoursSuitableEvent>(companyOutbox.Payload);

                if (companyWorkingHoursSuitableEvent != null)
                {
                    await _publishEndpoint.Publish(companyWorkingHoursSuitableEvent);
                    companyOutbox.ProcessedDate = DateTime.Now;
                    await _companyDbContext.SaveChangesAsync();
                }
            }

        }
    }
}
