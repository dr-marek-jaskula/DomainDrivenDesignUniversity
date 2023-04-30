using Shopway.Domain.Abstractions;
using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Persistence.Utilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products;

public sealed record CommonProductOrder : ISortBy<Product>
{
    public IReadOnlyCollection<string> AllowedSortProperties { get; init; } = CommonAllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = CommonProductSortProperties;

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Apply(SortProperties);
    }
}