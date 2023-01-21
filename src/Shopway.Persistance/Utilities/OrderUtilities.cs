using Shopway.Domain.Utilities;
using Shopway.Domain.Enums;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;

namespace Shopway.Persistence.Utilities;

public static class OrderUtilities
{
    public static IOrderedQueryable<TEntity> Order<TEntity>(this IQueryable<TEntity> queryable, ISortBy order)
        where TEntity : class, IEntity
    {
        return queryable
            .SortBy(order.SortBy.Property, order.SortBy.Direction)
            .ThenSortBy(order.ThenBy.Property, order.ThenBy.Direction);
    }

    private static IOrderedQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> queryable, string property, SortDirection sortDirection)
    {
        return sortDirection is SortDirection.Ascending
            ? queryable.OrderBy(property)
            : queryable.OrderByDescending(property);
    }

    private static IOrderedQueryable<TEntity> ThenSortBy<TEntity>(this IOrderedQueryable<TEntity> queryable, string property, SortDirection sortDirection)
    {
        return sortDirection is SortDirection.Ascending
            ? queryable.ThenBy(property)
            : queryable.ThenByDescending(property);
    }
}