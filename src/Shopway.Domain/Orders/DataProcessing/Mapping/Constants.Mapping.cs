using Shopway.Domain.Orders;
using static Shopway.Domain.Common.Utilities.CollectionUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Mapping
    {
        public static class OrderHeader
        {
            public readonly static IReadOnlyCollection<string> AllowedOrderHeaderMappingProperties = AsReadOnlyCollection
            (
                 nameof(Orders.OrderHeader.Id),
                 nameof(Orders.OrderHeader.Status),
                 nameof(Orders.OrderHeader.SoftDeleted),
                 nameof(Orders.OrderHeader.CreatedBy),
                 nameof(Orders.OrderHeader.CreatedOn),
                 nameof(Orders.OrderHeader.TotalDiscount),
                 $"{nameof(Orders.OrderHeader.Payments)}.{nameof(Payment.CreatedBy)}",
                 $"{nameof(Orders.OrderHeader.Payments)}.{nameof(Payment.Status)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(OrderLine.Amount)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(OrderLine.LineDiscount)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(OrderLine.ProductSummary)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(OrderLine.ProductSummary)}.{nameof(Orders.OrderLine.ProductSummary.ProductName)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(OrderLine.ProductSummary)}.{nameof(Orders.OrderLine.ProductSummary.Revision)}"
            );
        }
    }
}
