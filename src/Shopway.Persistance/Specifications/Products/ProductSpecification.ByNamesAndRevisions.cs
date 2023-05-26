using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByNamesAndRevisions : SpecificationBase<Product, ProductId>
    {
        private ByNamesAndRevisions() : base()
        {
        }

        internal static SpecificationBase<Product, ProductId> Create(IList<string> productNames, IList<string> productRevisions)
        {
            return new ByNamesAndRevisions()
                .AddFilters
                (
                    product => productNames.Contains(product.ProductName.Value),
                    product => productRevisions.Contains(product.Revision.Value)
                );
        }
    }
}