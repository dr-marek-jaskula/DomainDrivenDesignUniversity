using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions.Common;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.QueryableUtilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products.Filtering;

public sealed record ProductDynamicFilter : IDynamicFilter<Product>
{
    public static IReadOnlyCollection<string> AllowedFilterProperties { get; } = AllowedProductFilterProperties;
    public static IReadOnlyCollection<string> AllowedFilterOperations { get; } = AllowedProductFilterOperations;

    public IList<FilterByEntry> FilterProperties { get; init; } = EmptyList<FilterByEntry>();

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