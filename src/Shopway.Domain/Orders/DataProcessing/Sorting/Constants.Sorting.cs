using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Enums;
using static Shopway.Domain.Common.Utilities.CollectionUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Sorting
    {
        public static class OrderHeader
        {
            public readonly static IReadOnlyCollection<string> AllowedOrderHeaderSortProperties = AsReadOnlyCollection
            (
                 nameof(Orders.OrderHeader.Status),
                 nameof(Orders.OrderHeader.TotalDiscount)
            );

            public readonly static IReadOnlyCollection<string> CommonAllowedOrderHeaderSortProperties = AsReadOnlyCollection
            (
                 nameof(Orders.OrderHeader.Status),
                 nameof(Orders.OrderHeader.TotalDiscount)
            );

            public readonly static IList<SortByEntry> CommonOrderHeaderSortProperties =
            [
                new SortByEntry() { PropertyName = nameof(Orders.OrderHeader.TotalDiscount), SortDirection = SortDirection.Ascending, SortPriority = 1 }
            ];
        }
    }
}
