using Shared.Events.Common;

namespace Shared.Events.CompanyEvents;

public class CompanyWorkingHoursNotSuitableEvent : IEvent
{
    public string OrderId { get; set; }
    public string? Message { get; set; }
}