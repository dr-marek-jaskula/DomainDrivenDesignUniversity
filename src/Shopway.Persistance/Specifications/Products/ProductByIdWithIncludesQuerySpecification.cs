using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithIncludesQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByIdWithIncludesQuerySpecification() : base()
    {
    }

    public static SpecificationBase<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
    {
        var specification = new ProductByIdWithIncludesQuerySpecification();

        specification
            .AddFilters(product => product.Id == productId);

        specification
            .AddIncludes(includes);

        specification.IsSplitQuery = true;

        return specification;
    }
}