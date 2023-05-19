using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithReviewsQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByIdWithReviewsQuerySpecification() : base()
    {
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductId productId)
    {
        return new ProductByIdWithReviewsQuerySpecification()
            .AddFilters(product => product.Id == productId)
            .AddIncludes(product => product.Reviews);
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductId productId, ReviewId reviewId)
    {
        return new ProductByIdWithReviewsQuerySpecification()
            .AddFilters(product => product.Id == productId)
            .AddIncludes(product => product.Reviews.Where(review => review.Id == reviewId));
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductId productId, Title title)
    {
        return new ProductByIdWithReviewsQuerySpecification()
            .AddFilters(product => product.Id == productId)
            .AddIncludes(product => product.Reviews.Where(review => review.Title.Value == title.Value));
    }
}