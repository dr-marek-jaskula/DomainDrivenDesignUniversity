using Shopway.Domain.Abstractions;
using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products.Sorting;

public sealed record CommonProductOrder : ISortBy<Product>
{
    public IReadOnlyCollection<string> AllowedSortProperties { get; init; } = CommonAllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = CommonProductSortProperties;

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}