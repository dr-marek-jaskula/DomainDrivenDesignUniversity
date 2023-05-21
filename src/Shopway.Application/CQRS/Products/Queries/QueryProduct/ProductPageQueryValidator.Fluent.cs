using Shopway.Domain.Common;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryValidator : PageQueryValidator<ProductPageQuery, ProductResponse, ProductFilter, ProductOrder, Page>
{
    public ProductPageQueryValidator()
        : base()
    {
    }
}