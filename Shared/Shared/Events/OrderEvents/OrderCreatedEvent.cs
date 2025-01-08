using Shared.Events.Common;
using Shared.Messages;

namespace Shared.Events.OrderEvents;

public class OrderCreatedEvent : IEvent
{
    public string OrderId { get; set; }
    public string BuyerId { get; set; }
    public string CompanyId { get; set; }
    public decimal totalprice { get; set; }
    public string IdempotenToken { get; set; }
    public ICollection<OrderItemMessage> OrderItemMessages { get; set; }
}