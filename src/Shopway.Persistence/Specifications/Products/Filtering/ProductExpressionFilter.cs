using Shopway.Domain.Abstractions;
using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using static Shopway.Domain.Utilities.QueryableUtilities;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products.Filtering;

public sealed record ProductExpressionFilter : IExpressionFilter<Product>
{
    public IReadOnlyCollection<string> AllowedFilterProperties { get; init; } = AllowedProductFilterProperties;

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