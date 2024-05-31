using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal static partial class ProductSpecification
{
    internal static partial class ByKey
    {
        internal static Specification<Product, ProductId> Create(ProductKey productKey)
        {
            return Specification<Product, ProductId>.New()
                .AddFilters
                (
                    product => (string)(object)product.ProductName == productKey.ProductName,
                    product => (string)(object)product.Revision == productKey.Revision
                )
                .AddTag(QueryProductByKey);
        }
    }
}
