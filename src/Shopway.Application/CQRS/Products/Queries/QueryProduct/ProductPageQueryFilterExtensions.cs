using Shopway.Domain.Entities;
using static Shopway.Application.CQRS.Products.Queries.QueryProduct.ProductPageQuery;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

public static class ProductPageQueryFilterExtensions
{
    public static IQueryable<Product> Filter(this IQueryable<Product> queryable, ProductPageFilter filter)
    {
        return queryable
            .FilterByProductName(filter)
            .FilterByRevision(filter)
            .FilterByPrice(filter)
            .FilterByUomCode(filter);
    }

    private static IQueryable<Product> FilterByProductName(this IQueryable<Product> queryable, ProductPageFilter filter)
    {
        return queryable.Filter(filter.ByProductName, product => product.ProductName.Value.Contains(filter.ProductName!));
    }

    private static IQueryable<Product> FilterByRevision(this IQueryable<Product> queryable, ProductPageFilter filter)
    {
        return queryable.Filter(filter.ByRevision, product => product.Revision.Value.Contains(filter.Revision!));
    }

    private static IQueryable<Product> FilterByPrice(this IQueryable<Product> queryable, ProductPageFilter filter)
    {
        return queryable.Filter(filter.ByPrice, product => product.Price.Value == filter.Price!);
    }

    private static IQueryable<Product> FilterByUomCode(this IQueryable<Product> queryable, ProductPageFilter filter)
    {
        return queryable.Filter(filter.ByUomCode, product => product.UomCode.Value.Contains(filter.UomCode!));
    }
}