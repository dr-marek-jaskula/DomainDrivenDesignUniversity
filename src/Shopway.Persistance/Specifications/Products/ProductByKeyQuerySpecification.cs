using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByKeyQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByKeyQuerySpecification() : base()
    {
    }

    public static SpecificationBase<Product, ProductId> Create(ProductKey key)
    {
        var specification = new ProductByKeyQuerySpecification();

        specification
            .AddFilters
            (
                product => product.ProductName.Value == key.ProductName,
                product => product.Revision.Value == key.Revision
            );

        return specification;
    }
}