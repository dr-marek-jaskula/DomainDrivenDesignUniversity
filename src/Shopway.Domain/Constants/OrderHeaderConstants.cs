using Shopway.Domain.Enums;
using static Shopway.Domain.Enums.OrderStatus;

namespace Shopway.Domain.Constants;

public static class OrderHeaderConstants
{
    public static List<(OrderStatus source, OrderStatus destination)> AvailableOrderStatusChangeCombinations = new()
    {
        (New, InProgress),
        (InProgress, Shipped),
        (Shipped, Delivered),
        (New, OnHold),
        (New, Rejected),
        (InProgress, OnHold),
        (InProgress, Rejected),
        (OnHold, New)
    };
}