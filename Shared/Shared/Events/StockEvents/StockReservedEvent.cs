namespace Shared.Events.StockEvents;

public class StockReservedEvent
{
    public string OrderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public decimal TotalPrice { get; set; } 
    public string BuyerId { get; set; }  
}