using Shared.Events.Common;
using Shared.Messages;

namespace Shared.Events.CompanyEvents;

public class CompanyWorkingHoursSuitableEvent : IEvent
{
    //public string OrderId { get; set; }
    public ICollection<OrderItemMessage> OrderItems { get; set; }
    public string? Message { get; set; }
    public string OrderId { get; set; }
    public string BuyerId { get; set; }
    public string CompanyId { get; set; }
    public string IdempotenToken { get; set; }
    public decimal totalprice { get; set; }
}