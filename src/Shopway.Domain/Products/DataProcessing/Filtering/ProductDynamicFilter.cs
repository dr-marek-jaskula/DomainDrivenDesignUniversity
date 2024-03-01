using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Common.Utilities.QueryableUtilities;
using static Shopway.Domain.Constants.Constants.Filtering.Product;

namespace Shopway.Domain.Products.DataProcessing.Filtering;

public sealed record ProductDynamicFilter : IDynamicFilter<Product>
{
    public static IReadOnlyCollection<string> AllowedFilterProperties { get; } = AllowedProductFilterProperties;
    public static IReadOnlyCollection<string> AllowedFilterOperations { get; } = AllowedProductFilterOperations;

    public required IList<FilterByEntry> FilterProperties { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        if (FilterProperties.IsNullOrEmpty())
        {
            return queryable;
        }

        return queryable
            .Where(FilterProperties);
    }
}