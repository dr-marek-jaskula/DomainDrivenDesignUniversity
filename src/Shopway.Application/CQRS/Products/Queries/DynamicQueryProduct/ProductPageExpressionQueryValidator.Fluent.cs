using Shopway.Application.Abstractions;
using Shopway.Domain.Common;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;

namespace Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

internal sealed class ProductPageDynamicQueryValidator : PageQueryValidator<ProductPageDynamicQuery, ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, Page>
{
    public ProductPageDynamicQueryValidator()
        : base()
    {
    }
}