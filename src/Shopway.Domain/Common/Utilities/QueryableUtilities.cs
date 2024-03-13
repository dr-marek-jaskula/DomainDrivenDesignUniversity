using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using static Shopway.Domain.Constants.Constants.Type;

namespace Shopway.Domain.Common.Utilities;

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

    public static IQueryable<TEntity> Where<TEntity>
    (
        this IQueryable<TEntity> queryable,
        IList<FilterByEntry> filterEntries,
        ILikeProvider<TEntity>? likeProvider = null
    )
        where TEntity : class, IEntity
    {
        var expression = filterEntries.CreateFilterExpression(likeProvider);

        return queryable
            .Where(expression);
    }

    /// <summary>
    /// This method generates expressions that will be use to filter entities by their ValueObjects with single inner value. 
    /// For primitive types use simplified version of this method
    /// </summary>
    public static Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(this IList<FilterByEntry> filterEntries, ILikeProvider<TEntity>? likeProvider = null)
        where TEntity : class, IEntity
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        List<Expression> filterEntryExpressions = [];

        foreach (var filterEntry in filterEntries)
        {
            //We create expression that is logic OR of all provided predicates
            Expression? filterEntryExpression = null;

            foreach (var predicate in filterEntry.Predicates)
            {
                var memberExpression = parameter.ToMemberExpression(predicate.PropertyName);

                Type innerTypeForValueObjectOrCurrentTypeForPrimitive;
                Expression convertedPropertyToFilterOn;

                if (memberExpression.Type.IsValueObject())
                {
                    innerTypeForValueObjectOrCurrentTypeForPrimitive = memberExpression.GetValueObjectInnerType();
                    convertedPropertyToFilterOn = memberExpression.ConvertInnerValueToInnerTypeAndObject(innerTypeForValueObjectOrCurrentTypeForPrimitive);
                }
                else
                {
                    innerTypeForValueObjectOrCurrentTypeForPrimitive = memberExpression.Type;
                    convertedPropertyToFilterOn = memberExpression;
                }

                var convertedValueForFiltering = innerTypeForValueObjectOrCurrentTypeForPrimitive.ToConvertedExpression(predicate.Value);

                var isBinaryOperation = Enum.TryParse(predicate.Operation, out ExpressionType expressionType);

                if (isBinaryOperation)
                {
                    var newBinary = Expression.MakeBinary(expressionType, convertedPropertyToFilterOn, convertedValueForFiltering);

                    filterEntryExpression = filterEntryExpression is null
                        ? newBinary
                        : Expression.MakeBinary(ExpressionType.OrElse, filterEntryExpression, newBinary);

                    continue;
                }

                if (predicate.Operation == "Like")
                {
                    if (likeProvider is null)
                    {
                        throw new ArgumentNullException(nameof(likeProvider), "When Like operation is required, than like provider must not be null");
                    }

                    var newLike = likeProvider.CreateLikeExpression(parameter, predicate.PropertyName, $"{predicate.Value}");

                    filterEntryExpression = filterEntryExpression is null
                        ? newLike
                        : Expression.OrElse(filterEntryExpression, newLike);

                    continue;
                }

                var method = StringType.GetMethod(predicate.Operation, [StringType]);
                var newMethodCallExpression = Expression.Call(convertedPropertyToFilterOn, method!, Expression.Constant(predicate.Value));

                filterEntryExpression = filterEntryExpression is null
                    ? newMethodCallExpression
                    : Expression.OrElse(filterEntryExpression, newMethodCallExpression);
            }

            filterEntryExpressions.Add(filterEntryExpression!);
        }

        //We create expression that is logic AND of all provided Filter Entries
        Expression? expression = null;
        foreach (var filterEntryExpression in filterEntryExpressions)
        {
            expression = expression is null
                    ? filterEntryExpression
                    : Expression.AndAlso(expression!, filterEntryExpression);
        }

        return Expression.Lambda<Func<TEntity, bool>>(expression!, parameter);
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

    public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> queryable, IOffsetPage page)
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
            .SetSortPriorities()
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

    public static IQueryable<DataTransferObject> Map<TInput>
    (
        this IQueryable<TInput> queryable,
        IList<MappingEntry> mappingEntries
    )
        where TInput : class, IEntity
    {
        var lambdaExpression = DataTransferObject.CreateExpression<TInput>(mappingEntries);

        return queryable
            .Select(lambdaExpression);
    }
}