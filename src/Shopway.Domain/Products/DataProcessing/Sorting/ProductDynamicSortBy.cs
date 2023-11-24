using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using static Shopway.Domain.Constants.Constants.Sorting.Product;

namespace Shopway.Domain.Products.DataProcessing.Sorting;

public sealed record ProductDynamicSortBy : IDynamicSortBy<Product>
{
    public static IReadOnlyCollection<string> AllowedSortProperties { get; } = AllowedProductSortProperties;

    public required IList<SortByEntry> SortProperties { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}