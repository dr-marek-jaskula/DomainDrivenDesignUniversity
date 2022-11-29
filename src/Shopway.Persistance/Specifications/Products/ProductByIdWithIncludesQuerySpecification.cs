using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.StronglyTypedIds;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithIncludesQuerySpecification : BaseQuerySpecification<Product, ProductId>
{
    private ProductByIdWithIncludesQuerySpecification() : base()
    {
    }

    public static BaseSpecification<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
    {
        var specification = new ProductByIdWithIncludesQuerySpecification();

        specification.AddFilters(product => product.Id == productId);

        specification.AddIncludes(includes);

        specification.IsSplitQuery = true;

        specification
            .OrderByWithDirection(x => x.ProductName, SortDirection.Ascending)
            .ThenByWithDirection(x => x.Price, SortDirection.Descending);

        return specification;
    }
}