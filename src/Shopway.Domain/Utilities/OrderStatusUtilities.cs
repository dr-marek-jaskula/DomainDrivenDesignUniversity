using Shopway.Domain.Enums;
using static Shopway.Domain.Common.DomainConstants;

namespace Shopway.Domain.Utilities;

public static class OrderStatusUtilities
{
    public static bool CanBeChangedTo(this OrderStatus source, OrderStatus destination)
    {
        return AvailableOrderStatusChangeCombinations.Contains((source, destination));
    }
}