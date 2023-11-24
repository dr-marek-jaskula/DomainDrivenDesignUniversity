using Shopway.Domain.Common.DataProcessing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products.DataProcessing.Sorting;
using Shopway.Domain.Products.DataProcessing.Filtering;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;

public sealed record ProductOffsetPageDynamicQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}