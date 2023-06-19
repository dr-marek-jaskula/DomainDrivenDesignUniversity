using Shopway.Domain.Abstractions;
using Shopway.Domain.Enums;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Shopway.Domain.Common;
using static Shopway.Domain.Utilities.ExpressionUtilities;
using static Shopway.Domain.Constants.TypeConstants;

namespace Shopway.Domain.Utilities;

public static class QueryableUtilities
{
    public static IOrderedQueryable<TEntity> OrderBy<TEntity>
    (
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> sortBy,
        SortDirection sortDirection
    )
    {
        return sortDirection is SortDirection.Ascending
            ? queryable.OrderBy(sortBy)
            : queryable.OrderByDescending(sortBy);
    }

    public static IOrderedQueryable<TEntity> ThenBy<TEntity>
    (
        this IOrderedQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> sortBy,
        SortDirection sortDirection
    )
    {
        return sortDirection is SortDirection.Ascending
            ? queryable.ThenBy(sortBy)
            : queryable.ThenByDescending(sortBy);
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

    public static IQueryable<TResponse> Where<TResponse>
    (
        this IQueryable<TResponse> queryable,
        IList<FilterByEntry> filterEntries
    )
    {
        var parameter = Expression.Parameter(typeof(TResponse));
        BinaryExpression? binaryExpression = null;
        MethodCallExpression? nonBinaryExpression = null;

        foreach (var filter in filterEntries)
        {
            var memberExpression = parameter.ToMemberExpression(filter.PropertyName);
            var innerType = memberExpression.GetValueObjectInnerType();
            var convertedValueForFiltering = innerType.ToConvertedExpression(filter.Value);

            Expression convertedPropertyToFilterOn = Expression.Property(memberExpression, "Value");
            convertedPropertyToFilterOn = Expression.Convert(memberExpression, typeof(object));
            convertedPropertyToFilterOn = Expression.Convert(convertedPropertyToFilterOn, innerType);

            var isBinaryOperation = Enum.TryParse(filter.Operation, out ExpressionType expressionType);

            if (isBinaryOperation)
            {
                var newBinary = Expression.MakeBinary(expressionType, convertedPropertyToFilterOn, convertedValueForFiltering);

                binaryExpression = binaryExpression is null
                    ? newBinary
                    : Expression.MakeBinary(ExpressionType.AndAlso, binaryExpression, newBinary);

                continue;
            }

            var method = StringType.GetMethod(filter.Operation, new[] { StringType });
            var newNonBinaryExpression = Expression.Call(convertedPropertyToFilterOn, method!, Expression.Constant(filter.Value));

            nonBinaryExpression = nonBinaryExpression is null
                ? newNonBinaryExpression
                : Expression.Call(convertedPropertyToFilterOn, method!, Expression.Constant(filter.Value));
        }

        Expression<Func<TResponse, bool>> lambdaExpression = CreateLambdaExpression<TResponse>(parameter, binaryExpression, nonBinaryExpression);

        return queryable
            .Where(lambdaExpression);
    }

    public static IQueryable<TEntity> Page<TEntity>
    (
        this IQueryable<TEntity> queryable,
        int pageSize,
        int pageNumber
    )
    {
        return queryable
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
    }

    public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> queryable, IPage page)
    {
        return queryable.Page(page.PageSize, page.PageNumber);
    }

    public static IQueryable<TEntity> SortBy<TEntity, TValue>
    (
        this IQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TValue>> expression
    )
    {
        return sortDirection switch
        {
            SortDirection.Ascending => queryable.OrderBy(expression),
            SortDirection.Descending => queryable.OrderByDescending(expression),
            _ => queryable
        };
    }

    public static IOrderedQueryable<TEntity> ThenSortBy<TEntity, TValue>
    (
        this IOrderedQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TValue>> expression
    )
    {
        return sortDirection switch
        {
            SortDirection.Ascending => queryable.ThenBy(expression),
            SortDirection.Descending => queryable.ThenByDescending(expression),
            _ => queryable
        };
    }

    public static IQueryable<TResponse> Sort<TResponse>
    (
        this IQueryable<TResponse> queryable,
        IEnumerable<SortByEntry> sortProperties
    )
    {
        var sortedProperties = sortProperties
            .Distinct()
            .OrderBy(x => x.SortPriority);

        var firstElement = sortedProperties.FirstOrDefault();

        if (firstElement is null)
        {
            return queryable;
        }

        queryable = queryable
            .SortByValueObjectName(firstElement.SortDirection, firstElement.PropertyName);

        foreach (var item in sortedProperties.Skip(1))
        {
            queryable = ((IOrderedQueryable<TResponse>)queryable)
                .ThenSortByValueObjectName(item.SortDirection, item.PropertyName);
        }

        return queryable;
    }

    public static IQueryable<TEntity> SortByValueObjectName<TEntity>
    (
        this IQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        string propertyName
    )
    {
        return sortDirection switch
        {
            SortDirection.Ascending => queryable.OrderBy(propertyName),
            SortDirection.Descending => queryable.OrderBy($"{propertyName} DESC"),
            _ => queryable
        };
    }

    public static IOrderedQueryable<TEntity> ThenSortByValueObjectName<TEntity>
    (
        this IOrderedQueryable<TEntity> queryable,
        SortDirection? sortDirection,
        string propertyName
    )
    {
        return sortDirection switch
        {
            SortDirection.Ascending => queryable.ThenBy(propertyName),
            SortDirection.Descending => queryable.ThenBy($"{propertyName} DESC"),
            _ => queryable
        };
    }
}