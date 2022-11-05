using Shopway.Domain.Entities;

namespace Shopway.Persistence.Specifications;

public sealed class ProductByIdWithReviewsSpecification : Specification<Product>
{
    public ProductByIdWithReviewsSpecification(Guid productId)
        : base(isAsNoTracking: true, criterias: product => product.Id == productId)
    {
        AddInclude(product => product.Reviews);
    }
}

//TODO: make filtering on demand if needed!