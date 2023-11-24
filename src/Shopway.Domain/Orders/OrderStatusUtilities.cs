using Shopway.Domain.Enums;
using static Shopway.Domain.Constants.Constants.OrderHeader;

namespace Shopway.Domain.Orders;

public static class OrderStatusUtilities
{
    public static bool CanBeChangedTo(this OrderStatus source, OrderStatus destination)
    {
        return AvailableOrderStatusChangeCombinations.Contains((source, destination));
    }
}