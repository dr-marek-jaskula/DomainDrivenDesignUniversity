using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByKey : SpecificationBase<Product, ProductId>
    {
        internal static SpecificationBase<Product, ProductId> Create(ProductKey key)
        {
            return new ByKey()
                .AddFilters
                (
                    product => product.ProductName.Value == key.ProductName,
                    product => product.Revision.Value == key.Revision
                );
        }
    }
}