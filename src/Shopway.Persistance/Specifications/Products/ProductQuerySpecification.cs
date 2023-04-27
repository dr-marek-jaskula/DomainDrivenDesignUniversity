using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductQuerySpecification<TResposnse> : SpecificationWithMappingBase<Product, ProductId, TResposnse>
{
    private ProductQuerySpecification() : base()
    {
    }

    public static SpecificationWithMappingBase<Product, ProductId, TResposnse> Create(IFilter<Product>? filter, ISortBy<Product>? sortBy, IPage page, Expression<Func<Product, TResposnse>>? select, params Expression<Func<Product, object>>[] includes)
    {
        var specification = new ProductQuerySpecification<TResposnse>();

        specification
            .AddIncludes(includes);

        specification
            .AddFilters(filter);

        specification
            .AddPage(page);

        specification
            .AddOrder(sortBy);

        specification
            .AddSelect(select);

        //Alternative way
        //specification
        //    .OrderBy(x => x.ProductName.Value, SortDirection.Ascending)
        //    .ThenBy(x => x.Price.Value, SortDirection.Descending);

        return specification;
    }
}