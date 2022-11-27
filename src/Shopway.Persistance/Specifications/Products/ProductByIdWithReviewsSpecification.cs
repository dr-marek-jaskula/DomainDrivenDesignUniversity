using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithReviewsSpecification : BaseSpecification<Product, ProductId>
{
    private ProductByIdWithReviewsSpecification()
    {
    }

    public static BaseSpecification<Product, ProductId> Create(ProductId productId)
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