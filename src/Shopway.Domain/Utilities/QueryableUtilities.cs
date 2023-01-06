using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Application.CQRS;

public static class QueryableUtilities
{
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

    public static IQueryable<TEntity> Filter<TEntity>
    (
        this IQueryable<TEntity> queryable,
        bool predicate,
        Expression<Func<TEntity, bool>> ifPredicateIsTrue,
        Expression<Func<TEntity, bool>> ifPredicateIsFalse
    )
    {
        return predicate
            ? queryable.Where(ifPredicateIsTrue)
            : queryable.Where(ifPredicateIsFalse);
    }

    public static IQueryable<TEntity> SortBy<TEntity>
    (
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, bool>> expression,
        SortDirection sortDirection
    )
    {
        return sortDirection is SortDirection.Ascending 
            ? queryable.OrderBy(expression)
            : queryable.OrderByDescending(expression);
    }

    public static IQueryable<TEntity> Page<TEntity>
    (
        this IQueryable<TEntity> queryable,
        int skip,
        int take
    )
    {
        return queryable
            .Skip(skip)
            .Take(take);
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
    {
        return queryable.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, string propertyName)
    {
        return queryable.OrderByDescending(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propertyName)
    {
        return queryable.ThenBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> queryable, string propertyName)
    {
        return queryable.ThenByDescending(ToLambda<T>(propertyName));
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}