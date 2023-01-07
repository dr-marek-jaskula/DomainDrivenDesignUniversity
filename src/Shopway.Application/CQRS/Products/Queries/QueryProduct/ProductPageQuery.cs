using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

public sealed record ProductPageQuery
(
    int PageNumber,
    int PageSize
)
    : IPageQuery<ProductResponse, ProductFilter, ProductOrder>
{
    public ProductFilter? Filter { get; init; }
    public ProductOrder? Order { get; init; }
}