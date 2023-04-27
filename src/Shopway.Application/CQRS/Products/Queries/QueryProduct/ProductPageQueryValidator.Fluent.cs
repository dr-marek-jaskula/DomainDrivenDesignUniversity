using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryValidator : PageQueryValidator<ProductPageQuery, ProductResponse, ProductFilter, ProductOrder, Page>
{
    public ProductPageQueryValidator()
        : base()
    {
    }
}