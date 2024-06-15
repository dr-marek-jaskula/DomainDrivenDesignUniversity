using Shopway.Domain.Products;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal static partial class ProductSpecification
{
    internal static partial class ByNamesAndRevisions
    {
        internal static Specification<Product, ProductId> Create(IList<string> productNames, IList<string> productRevisions)
        {
            return Specification<Product, ProductId>.New()
                .AddFilters
                (
                    product => productNames.Contains((string)(object)product.ProductName),
                    product => productRevisions.Contains((string)(object)product.Revision)
                )
                .AddTag(QueryProductByProductNamesAndProductRevisions);
        }
    }
}
