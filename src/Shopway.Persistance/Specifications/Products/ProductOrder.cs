using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Utilities;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy<Product>
{
    public IReadOnlyCollection<string> AllowedSortProperties { get; init; } = AllowedProductSortProperties;

    public IList<SortByEntry> SortProperties { get; init; } = Empty<SortByEntry>();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Sort(SortProperties);
    }
}