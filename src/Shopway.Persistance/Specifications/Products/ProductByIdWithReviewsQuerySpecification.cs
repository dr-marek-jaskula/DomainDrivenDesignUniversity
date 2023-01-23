using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithReviewsQuerySpecification : BaseSpecification<Product, ProductId>
{
    private ProductByIdWithReviewsQuerySpecification() : base()
    {
    }

    public static BaseSpecification<Product, ProductId> Create(ProductId productId)
    {
        var specification = new ProductByIdWithReviewsQuerySpecification();

        specification
            .AddFilters(product => product.Id == productId);

        specification
            .AddIncludes(product => product.Reviews);

        return specification;
    }
}