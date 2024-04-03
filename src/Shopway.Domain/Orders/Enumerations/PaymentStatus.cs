namespace Shopway.Domain.Orders.Enumerations;

public enum PaymentStatus
{
    NotReceived = 1,
    Confirmed,
    Failed,
    Received
}