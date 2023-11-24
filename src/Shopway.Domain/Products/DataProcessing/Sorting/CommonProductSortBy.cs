using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using static Shopway.Domain.Constants.Constants.Sorting.Product;

namespace Shopway.Domain.Products.DataProcessing.Sorting;

public sealed record CommonProductSortBy : IDynamicSortBy<Product>
{
    public static IReadOnlyCollection<string> AllowedSortProperties { get; } = CommonAllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = CommonProductSortProperties;

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}