using Shopway.Domain.Products;
using Shopway.Domain.EntityKeys;
using Shopway.Persistence.Abstractions;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

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
                )
                .AddTag(QueryProductByKey);
        }
    }
}