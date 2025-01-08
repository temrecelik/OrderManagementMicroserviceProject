namespace Shared.Events.PaymentEvents;

public class PaymentCompletedEvent
{
    public string OrderId { get; set; }
    public decimal TotalPrice { get; set; }
}