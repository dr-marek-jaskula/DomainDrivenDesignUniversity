using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using static Shopway.Domain.Constants.Constants.Sorting.Product;

namespace Shopway.Domain.Sorting.Products;

public sealed record CommonProductSortBy : IDynamicSortBy<Product>
{
    public static IReadOnlyCollection<string> AllowedSortProperties { get; } = CommonAllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = CommonProductSortProperties;

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}