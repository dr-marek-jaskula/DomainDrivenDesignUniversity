using Shopway.Domain.Abstractions.Common;
using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products.Sorting;

public sealed record CommonProductSortBy : IDynamicSortBy<Product>
{
    public IReadOnlyCollection<string> AllowedSortProperties { get; } = CommonAllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = CommonProductSortProperties;

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}