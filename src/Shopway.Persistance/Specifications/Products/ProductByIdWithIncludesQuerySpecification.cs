using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithIncludesQuerySpecification : BaseSpecification<Product, ProductId>
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
            .OrderBy(x => x.ProductName, SortDirection.Ascending)
            .ThenBy(x => x.Price, SortDirection.Descending);

        return specification;
    }
}