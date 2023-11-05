using Shopway.Domain.Common;
using Shopway.Application.Sorting.Products;
using Shopway.Application.Filering.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

public sealed record ProductOffsetPageQuery(OffsetPage Page) : IOffsetPageQuery<ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    public ProductStaticFilter? Filter { get; init; }
    public ProductStaticSortBy? SortBy { get; init; }
}