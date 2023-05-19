using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithReviewQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByIdWithReviewQuerySpecification() : base()
    {
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductId productId, ReviewId reviewId)
    {
        return new ProductByIdWithReviewQuerySpecification()
            .AddFilters(product => product.Id == productId)
            .AddIncludes(product => product.Reviews.Where(review => review.Id == reviewId));
    }
}