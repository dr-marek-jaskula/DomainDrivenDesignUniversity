using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Common.Utilities.QueryableUtilities;
using static Shopway.Domain.Constants.Constants.Filtering.OrderHeader;

namespace Shopway.Domain.Orders.DataProcessing.Filtering;

public sealed record OrderHeaderDynamicFilter : IDynamicFilter<OrderHeader>
{
    public static IReadOnlyCollection<string> AllowedFilterProperties { get; } = AllowedOrderHeaderFilterProperties;
    public static IReadOnlyCollection<string> AllowedFilterOperations { get; } = AllowedOrderHeaderFilterOperations;

    public IList<FilterByEntry> FilterProperties { get; init; } = [];

    public IQueryable<OrderHeader> Apply(IQueryable<OrderHeader> queryable, ILikeProvider<OrderHeader>? likeProvider = null)
    {
        if (FilterProperties.IsNullOrEmpty())
        {
            return queryable;
        }

        return queryable
            .Where(FilterProperties.CreateFilterExpression(likeProvider));
    }
}