using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.QueryOffsetPageProduct;

public sealed record ProductOffsetPageQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    public ProductStaticFilter? Filter { get; init; }
    public ProductStaticSortBy? SortBy { get; init; }
}