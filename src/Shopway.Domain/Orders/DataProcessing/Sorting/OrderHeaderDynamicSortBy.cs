using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Constants.Constants.Sorting.OrderHeader;

namespace Shopway.Domain.Orders.DataProcessing.Sorting;

public sealed record OrderHeaderDynamicSortBy : IDynamicSortBy<OrderHeader>
{
    public static IReadOnlyCollection<string> AllowedSortProperties { get; } = AllowedOrderHeaderSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = [];

    public IQueryable<OrderHeader> Apply(IQueryable<OrderHeader> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}
