using Shopway.Domain.Common;
using Shopway.Application.Sorting.Products;
using Shopway.Application.Filering.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;

public sealed record ProductOffsetPageDynamicQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}