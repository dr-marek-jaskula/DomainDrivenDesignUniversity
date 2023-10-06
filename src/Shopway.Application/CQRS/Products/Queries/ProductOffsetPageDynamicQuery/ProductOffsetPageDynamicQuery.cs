using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;

namespace Shopway.Application.CQRS.Products.Queries.DynamicOffsetProductQuery;

public sealed record ProductOffsetPageDynamicQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}