using Shopway.Domain.Entities;

namespace Shopway.Persistence;

public static partial class Helpers
{
    public static decimal CalculateTotal(Order? order)
    {
        if (order is null || order.Payment is null || order.Product is null)
            return 0;

        if (order.Payment.Discount is not null)
            return Math.Round(order.Product.Price * order.Amount * (1 - (decimal)order.Payment.Discount), 2);
        
        return Math.Round(order.Product.Price * order.Amount, 2);
    }
}