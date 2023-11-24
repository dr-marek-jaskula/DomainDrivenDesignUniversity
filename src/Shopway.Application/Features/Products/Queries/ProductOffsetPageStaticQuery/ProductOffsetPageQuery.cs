using Shopway.Domain.Common.DataProcessing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products.DataProcessing.Sorting;
using Shopway.Domain.Products.DataProcessing.Filtering;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

public sealed record ProductOffsetPageQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    public ProductStaticFilter? Filter { get; init; }
    public ProductStaticSortBy? SortBy { get; init; }
}