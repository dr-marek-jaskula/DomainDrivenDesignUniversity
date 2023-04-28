using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductQuerySpecification<TResponse> : SpecificationWithMappingBase<Product, ProductId, TResponse>
{
    private ProductQuerySpecification() : base()
    {
    }

    internal static SpecificationWithMappingBase<Product, ProductId, TResponse> Create
    (
        IFilter<Product>? filter, 
        ISortBy<Product>? sortBy, 
        IPage page, 
        Expression<Func<Product, TResponse>>? select, 
        params Expression<Func<Product, object>>[] includes
    )
    {
        return new ProductQuerySpecification<TResponse>()
            .AddSelect(select)
            .AddIncludes(includes)
            .AddFilter(filter)
            .AddPage(page)
            .AddOrder(sortBy)
            .AsMappingSpecification<Product, ProductId, TResponse>();
    }
}