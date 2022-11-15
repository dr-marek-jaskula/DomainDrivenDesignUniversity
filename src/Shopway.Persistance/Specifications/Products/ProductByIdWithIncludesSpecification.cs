using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithIncludesSpecification : BaseSpecification<Product>
{
    private ProductByIdWithIncludesSpecification()
    {
    }

    public static BaseSpecification<Product> Create(Guid productId, params Expression<Func<Product, object>>[] includes)
    {
        var specification = new ProductByIdWithIncludesSpecification();

        specification.AddFilters(product => product.Id == productId);

        specification.AddIncludes(includes);

        specification.IsAsNoTracking = true;
        specification.IsSplitQuery = true;

        specification
            .OrderByWithDirection(x => x.ProductName, SortDirection.Ascending)
            .ThenByWithDirection(x => x.Price, SortDirection.Descending);

        return specification;
    }
}