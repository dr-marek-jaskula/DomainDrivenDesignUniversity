using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductQuerySpecification() : base()
    {
    }

    public static SpecificationBase<Product, ProductId> Create(IFilter<Product>? filter, ISortBy<Product>? sortBy, params Expression<Func<Product, object>>[] includes)
    {
        var specification = new ProductQuerySpecification();

        specification
            .AddIncludes(includes);

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