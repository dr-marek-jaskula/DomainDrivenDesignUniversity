using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

public sealed record ProductPageQuery(Page Page) : IPageQuery<ProductResponse, ProductStaticFilter, ProductStaticSortBy, Page>
{
    public ProductStaticFilter? Filter { get; init; }
    public ProductStaticSortBy? SortBy { get; init; }
}