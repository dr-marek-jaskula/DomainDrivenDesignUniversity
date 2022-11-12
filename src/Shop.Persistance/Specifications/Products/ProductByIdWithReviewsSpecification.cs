using Shopway.Domain.Entities;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Specifications.Products;

public sealed class ProductByIdWithReviewsSpecification : BaseSpecification<Product>
{
    public static BaseSpecification<Product> Create(Guid productId)
    {
        var specification = new ProductByIdWithReviewsSpecification();

        specification.AddFilters(product => product.Id == productId);

        specification.AddIncludes(product => product.Reviews);

        specification.IsAsNoTracking = true;

        specification
            .OrderByWithDirection(x => x.ProductName, SortDirection.Ascending)
            .ThenByWithDirection(x => x.Price, SortDirection.Descending);

        return specification;
    }
}