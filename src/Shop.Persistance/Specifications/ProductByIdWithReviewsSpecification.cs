using Shopway.Domain.Entities;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Specifications;

public sealed class ProductByIdWithReviewsSpecification : Specification<Product>
{
    private ProductByIdWithReviewsSpecification()
    {
    }

    public static Specification<Product> Create(Guid productId)
    {
        var specification = new ProductByIdWithReviewsSpecification();

        specification.AddFilters(product => product.Id == productId);

        specification.AddIncludes(product => product.Reviews);

        specification.IsAsNoTracking = true;

        //TODO: examine this
        specification.AddOrderByWithDirections(
            (product => product.Id, SortDirection.Ascending), 
            (product => product.Revision, SortDirection.Ascending));

        return specification;
    }
}