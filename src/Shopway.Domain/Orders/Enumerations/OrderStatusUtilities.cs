using static Shopway.Domain.Constants.Constants.OrderHeader;
using static Shopway.Domain.Orders.Enumerations.OrderStatus;

namespace Shopway.Domain.Orders.Enumerations;

public static class OrderStatusUtilities
{
    public static bool CanBeChangedTo(this OrderStatus source, OrderStatus destination)
    {
        return AvailableOrderStatusChangeCombinations.Contains((source, destination));
    }

    public static bool NotSent(this OrderStatus orderStatus)
    {
        return orderStatus is New or InProgress or OnHold;
    }
}
