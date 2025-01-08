namespace Shared;

public static class RabbitMqSettings
{
    public const string CompanyOrderCreatedEventQueue = "company-order-craeted-event-queue";
    public const string ProductCompanyWorkingHoursSuitableEventQueue = "product-company-working-hours-suitable-event-queue";
    public const string StockProductExistEventEventQueue = "stock-product-exist-event-queue";
    public const string PaymentStockReservedEventQueue = "payment-stock-reserved-event-queue";
    public const string OrderPaymentCompletedEventQueue = "order-payment-completed-event-queue";
    public const string OrderCompanyNotWorkingHoursSuitableEventQueue = "order-company-not-working-hours-suitable-event-queue";
    public const string OrderStockNotReservedEvent = "order-stock-not-reserved-event";
    public const string OrderPaymentFailedEventQueue = "order-payment-failed-event-queue";
     
}