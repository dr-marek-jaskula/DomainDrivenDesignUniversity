using Shopway.Domain.Products;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal static partial class ProductSpecification
{
    internal static partial class Names
    {
        internal static SpecificationWithMapping<Product, ProductId, string> Create()
        {
            return SpecificationWithMapping<Product, ProductId, string>.New()
                .AddMapping(x => x.ProductName.Value)
                .ApplyDistinct()
                .AddTag(QueryProductNames)
                .AsMappingSpecification<string>();
        }
    }
}
