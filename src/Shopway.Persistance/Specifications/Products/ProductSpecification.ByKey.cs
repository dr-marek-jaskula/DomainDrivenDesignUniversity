using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.ValueObjects;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByKey : SpecificationBase<Product, ProductId>
    {
        internal static SpecificationBase<Product, ProductId> Create(ProductName productName, Revision revision)
        {
            return new ByKey()
                .AddFilters
                (
                    product => product.ProductName == productName,
                    product => product.Revision == revision
                );
        }
    }
}