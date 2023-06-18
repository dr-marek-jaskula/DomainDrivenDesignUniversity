using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;

namespace Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

public sealed record ProductPageExpressionQuery(Page Page) : IPageQuery<ProductResponse, ProductExpressionFilter, ProductOrder, Page>
{
    public ProductExpressionFilter? Filter { get; init; }
    public ProductOrder? Order { get; init; }
}