namespace Shared.Events.StockEvents;

public class StockNotReservedEvent
{
    public bool Success { get; set; }
    public string Message { get; set; }
}