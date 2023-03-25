using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithReviewsQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByIdWithReviewsQuerySpecification() : base()
    {
    }

    public static SpecificationBase<Product, ProductId> Create(ProductId productId)
    {
        var specification = new ProductByIdWithReviewsQuerySpecification();

        specification
            .AddFilters(product => product.Id == productId);

        specification
            .AddIncludes(product => product.Reviews);

        return specification;
    }
}