using System.Linq.Expressions;
using static Shopway.Domain.Common.Utilities.CollectionUtilities;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Filtering
    {
        public static class OrderHeader
        {
            private const string Like = nameof(Like);

            public readonly static IReadOnlyCollection<string> AllowedOrderHeaderFilterProperties = AsReadOnlyCollection
            (
                 nameof(Orders.OrderHeader.CreatedBy),
                 nameof(Orders.OrderHeader.CreatedOn),
                 nameof(Orders.OrderHeader.SoftDeleted),
                 nameof(Orders.OrderHeader.Status),
                 nameof(Orders.OrderHeader.TotalDiscount),
                 nameof(Orders.OrderHeader.OrderLines),
                 $"{nameof(Orders.OrderHeader.Payments)}.{nameof(Orders.Payment.Status)}",
                 $"{nameof(Orders.OrderHeader.Payments)}.{nameof(Orders.Payment.CreatedBy)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderHeader.OrderLines.Count)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderLine.Amount)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderLine.CreatedBy)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderLine.ProductSummary)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderLine.ProductSummary)}.{nameof(Orders.OrderLine.ProductSummary.ProductName)}",
                 $"{nameof(Orders.OrderHeader.OrderLines)}.{nameof(Orders.OrderLine.ProductSummary)}.{nameof(Orders.OrderLine.ProductSummary.Price)}"
            );

            public readonly static IReadOnlyCollection<string> AllowedOrderHeaderFilterOperations = AsList
            (
                 nameof(string.Contains),
                 Like,
                 nameof(string.StartsWith),
                 nameof(string.EndsWith),
                 $"Any.{Like}",
                 $"Any.{nameof(string.Contains)}"
            )
                .Concat(GetNamesOf<ExpressionType>())
                .Concat(GetNamesOf<ExpressionType>().Select(x => $"Any.{x}"))
                .Concat(GetNamesOf<ExpressionType>().Select(x => $"All.{x}"))
                .ToList()
                .AsReadOnly();
        }
    }
}