using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByNamesAndRevisionQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByNamesAndRevisionQuerySpecification() : base()
    {
    }

    internal static SpecificationBase<Product, ProductId> Create(IList<string> productNames, IList<string> productRevisions)
    {
        return new ProductByNamesAndRevisionQuerySpecification()
            .AddFilters
            (
                product => productNames.Contains(product.ProductName.Value),
                product => productRevisions.Contains(product.Revision.Value)
            );
    }
}