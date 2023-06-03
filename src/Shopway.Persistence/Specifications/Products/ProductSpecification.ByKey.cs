using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.EntityKeys;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByKey : SpecificationBase<Product, ProductId>
    {
        internal static SpecificationBase<Product, ProductId> Create(ProductKey productKey)
        {
            return new ByKey()
                .AddFilters
                (
                    product => (string)(object)product.ProductName == productKey.ProductName,
                    product => (string)(object)product.Revision == productKey.Revision
                );
        }
    }
}