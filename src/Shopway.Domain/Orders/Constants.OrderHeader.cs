using Shopway.Domain.Orders.Enumerations;
using static Shopway.Domain.Orders.Enumerations.OrderStatus;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static class OrderHeader
    {
        public readonly static List<(OrderStatus source, OrderStatus destination)> AvailableOrderStatusChangeCombinations =
        [
            (New, InProgress),
            (InProgress, Shipped),
            (Shipped, Delivered),
            (New, OnHold),
            (New, Rejected),
            (InProgress, OnHold),
            (InProgress, Rejected),
            (OnHold, New)
        ];
    }
}