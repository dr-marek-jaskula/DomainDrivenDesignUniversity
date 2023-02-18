using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Domain.Utilities;

public static class QueryableUtilities
{
    public static IOrderedQueryable<TEntity> OrderBy<TEntity>
    (
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> SortBy,
        SortDirection SortDirection
    )
    {
        return SortDirection is SortDirection.Ascending
            ? queryable.OrderBy(SortBy)
            : queryable.OrderByDescending(SortBy);
    }

    public static IOrderedQueryable<TEntity> ThenBy<TEntity>
    (
        this IOrderedQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> SortBy,
        SortDirection SortDirection
    )
    {
        return SortDirection is SortDirection.Ascending
            ? queryable.ThenBy(SortBy)
            : queryable.OrderByDescending(SortBy);
    }

    public static IQueryable<TEntity> Filter<TEntity>
    (
        this IQueryable<TEntity> queryable,
        bool applyFilter,
        Expression<Func<TEntity, bool>> expression
    )
    {
        return applyFilter 
            ? queryable.Where(expression) 
            : queryable;
    }

    public static IQueryable<TEntity> SortBy<TEntity, TValue>
    (
        this IQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TValue>> expression
    )
    {
        if (sortDirection is not null)
        {
            return sortDirection is SortDirection.Ascending
                ? queryable.OrderBy(expression)
                : queryable.OrderByDescending(expression);
        }

        return queryable;
    }

    public static IOrderedQueryable<TEntity> ThenSortBy<TEntity, TValue>
    (
        this IOrderedQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TValue>> expression
    )
    {
        if (sortDirection is not null)
        {
            return sortDirection is SortDirection.Ascending
                ? queryable.ThenBy(expression)
                : queryable.ThenByDescending(expression);
        }

        return queryable;
    }

    public static IQueryable<T> WhereIf<T>
    (
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        bool condition
    )
    {
        if (condition)
        {
            return queryable.Where(predicate);
        }

        return queryable;
    }
}