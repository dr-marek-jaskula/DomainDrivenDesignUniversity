namespace Shopway.Domain.Orders.Enumerations;

public enum PaymentStatus
{
    NotReceived = 1,
    Confirmed = 2,
    Canceled = 3,
    Failed = 4,
    Received = 5
}