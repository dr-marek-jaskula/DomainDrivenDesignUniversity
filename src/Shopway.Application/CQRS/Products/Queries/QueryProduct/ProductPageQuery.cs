using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

public sealed record ProductPageQuery
(
    Page Page
)
    : IPageQuery<ProductResponse, ProductFilter, ProductOrder, Page>
{
    public ProductFilter? Filter { get; init; }
    public ProductOrder? Order { get; init; }
}