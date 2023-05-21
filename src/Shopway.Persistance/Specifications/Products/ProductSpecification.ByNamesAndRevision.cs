using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.EntityKeys;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByNamesAndRevision : SpecificationBase<Product, ProductId>
    {
        private ByNamesAndRevision() : base()
        {
        }

        internal static SpecificationBase<Product, ProductId> Create(IList<string> productNames, IList<string> productRevisions)
        {
            return new ByNamesAndRevision()
                .AddFilters
                (
                    product => productNames.Contains(product.ProductName.Value),
                    product => productRevisions.Contains(product.Revision.Value)
                );
        }
    }
}