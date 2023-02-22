using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductQuerySpecification : BaseSpecification<Product, ProductId>
{
    private ProductQuerySpecification() : base()
    {
    }

    public static BaseSpecification<Product, ProductId> Create(IFilter<Product>? filter, ISortBy<Product>? sortBy)
    {
        var specification = new ProductQuerySpecification();

        specification
            .AddIncludes(product => product.Reviews);

        specification
            .AddFilters(filter);

        specification
            .AddOrder(sortBy);

        //Alternative way
        //specification
        //    .OrderBy(x => x.ProductName.Value, SortDirection.Ascending)
        //    .ThenBy(x => x.Price.Value, SortDirection.Descending);

        return specification;
    }
}