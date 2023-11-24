using Shopway.Domain.Products;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Abstractions;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class Names : SpecificationWithMappingBase<Product, ProductId, string>
    {
        private Names() : base()
        {
        }

        internal static SpecificationWithMappingBase<Product, ProductId, string> Create()
        {
            return new Names()
                .AddMapping(x => x.ProductName.Value)
                .AddTag(QueryProductNames)
                .AsMappingSpecification<Product, ProductId, string>();
        }
    }
}