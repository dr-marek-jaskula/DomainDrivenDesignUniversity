using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByKeyQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByKeyQuerySpecification() : base()
    {
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductKey key)
    {
        return new ProductByKeyQuerySpecification()
            .AddFilters
            (
                product => product.ProductName.Value == key.ProductName,
                product => product.Revision.Value == key.Revision
            );
    }
}