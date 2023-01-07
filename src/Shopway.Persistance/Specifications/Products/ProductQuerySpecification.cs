using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductQuerySpecification : BaseSpecification<Product, ProductId>
{
    private ProductQuerySpecification() : base()
    {
    }

    public static BaseSpecification<Product, ProductId> Create(IFilter<Product>? filter, ISortBy? sortBy)
    {
        var specification = new ProductQuerySpecification();

        specification
            .AddIncludes(product => product.Reviews);

        specification
            .AddFilters(filter);

        specification
            .AddOrder(sortBy);

        return specification;
    }
}