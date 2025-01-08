namespace Shared.Events.PaymentEvents;

public class PaymentFailedEvent
{
    public string OrderId { get; set; }
    public string Message { get; set; }
}