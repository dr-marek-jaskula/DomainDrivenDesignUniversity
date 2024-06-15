namespace Shopway.Domain.Orders.Enumerations;

public enum OrderStatus
{
    New = 1,
    InProgress,
    Shipped,
    Delivered,
    OnHold,
    Rejected
}
