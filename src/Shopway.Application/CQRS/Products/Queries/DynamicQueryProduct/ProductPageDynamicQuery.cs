using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;

namespace Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

public sealed record ProductPageDynamicQuery(Page Page) : IPageQuery<ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, Page>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}