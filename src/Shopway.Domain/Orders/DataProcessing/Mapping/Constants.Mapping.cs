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
                 nameof(Orders.OrderHeader.OrderLines),
                 nameof(Orders.OrderHeader.SoftDeleted),
                 nameof(Orders.OrderHeader.CreatedBy),
                 nameof(Orders.OrderHeader.CreatedOn),
                 nameof(Orders.OrderHeader.TotalDiscount),
                 nameof(Orders.OrderHeader.Payment),
                 nameof(Orders.OrderHeader.Payment.CreatedBy),
                 nameof(Orders.OrderHeader.Payment.Status),
                 nameof(Orders.OrderLine.Amount),
                 nameof(Orders.OrderLine.ProductSummary),
                 nameof(Orders.OrderLine.LineDiscount),
                 nameof(Orders.OrderLine.ProductSummary.ProductName),
                 nameof(Orders.OrderLine.ProductSummary.Revision)
            );
        }
    }
}