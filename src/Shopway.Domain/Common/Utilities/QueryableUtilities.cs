using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using static Shopway.Domain.Common.DataProcessing.FilterByEntry;
using static Shopway.Domain.Common.Utilities.ExpressionUtilities;
using static Shopway.Domain.Constants.Constants.Type;

namespace Shopway.Domain.Common.Utilities;

public static class QueryableUtilities
{
    private static readonly FrozenDictionary<string, MethodInfo> _availableCollectionMethods = new Dictionary<string, MethodInfo>()
    {
        {
            nameof(Enumerable.Any), typeof(Enumerable)
                .GetTypeInfo()
                .GetDeclaredMethods(nameof(Enumerable.Any))
                .First(x => x.GetParameters().Length is 2)
        },
        {
            nameof(Enumerable.All), typeof(Enumerable)
                .GetTypeInfo()
                .GetDeclaredMethods(nameof(Enumerable.All))
                .First(x => x.GetParameters().Length is 2)
        }
    }.ToFrozenDictionary();

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
        var parameter = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);

        //We create expression that is logic AND of all provided Filter Entries
        Expression? expression = null;

        foreach (var filterEntry in filterEntries)
        {
            var filterEntryExpression = CreateFilterEntryExpression(likeProvider, parameter, filterEntry);

            expression = expression is null
                ? filterEntryExpression
                : Expression.AndAlso(expression!, filterEntryExpression);
        }

        return Expression.Lambda<Func<TEntity, bool>>(expression!, parameter);
    }

    private static Expression CreateFilterEntryExpression<TEntity>(ILikeProvider<TEntity>? likeProvider, ParameterExpression parameter, FilterByEntry filterEntry) 
        where TEntity : class, IEntity
    {
        //We create expression that is logic OR of all provided predicates
        Expression? filterEntryExpression = null;

        foreach (var predicate in filterEntry.Predicates)
        {
            int operationSeperatorIndex = predicate.Operation.IndexOf('.');
            var propertyOperation = predicate.Operation[(operationSeperatorIndex + 1)..];

            bool notCollectionOperation = operationSeperatorIndex is -1;

            if (notCollectionOperation)
            {
                var convertedMember = parameter
                    .ToMemberExpression(predicate.PropertyName)
                    .ToConvertedMember();

                var predicateExpression = CreatePredicateExpression(likeProvider, parameter, predicate, convertedMember, propertyOperation);

                filterEntryExpression = filterEntryExpression is null
                    ? predicateExpression
                    : Expression.OrElse(filterEntryExpression, predicateExpression);

                continue;
            }

            var member = parameter.ToMemberExpression(predicate.PropertyName, true);
            var memberName = member.Member.Name;
            var memberNameEndIndex = predicate.PropertyName.IndexOf(memberName) + memberName.Length;

            var collectionItemProperty = predicate.PropertyName[(memberNameEndIndex + 1)..];

            var colletionItemType = member.Type.GenericTypeArguments[0];

            var collectionOperation = predicate.Operation[..operationSeperatorIndex];

            var methodInfoForCollectionFilter = _availableCollectionMethods[collectionOperation]
                .MakeGenericMethod(colletionItemType);

            var collectionParameter = Expression.Parameter(colletionItemType, colletionItemType.Name);

            var collcetionConvertedMember = collectionParameter
                .ToMemberExpression(collectionItemProperty)
                .ToConvertedMember();

            var collectionPredicateExpression = CreatePredicateExpression(likeProvider, collectionParameter, predicate, collcetionConvertedMember, propertyOperation);

            var lambdaExpression = Expression.Lambda(collectionPredicateExpression, collectionParameter);
            var entityPredicateExpression = Expression.Call(null, methodInfoForCollectionFilter, member, lambdaExpression);

            filterEntryExpression = filterEntryExpression is null
                ? entityPredicateExpression
                : Expression.OrElse(filterEntryExpression, entityPredicateExpression);
        }

        return filterEntryExpression!;
    }

    private static Expression CreatePredicateExpression<TEntity>(ILikeProvider<TEntity>? likeProvider, ParameterExpression parameter, Predicate predicate, ConvertedMember convertedMember, string propertyOperation) 
        where TEntity : class, IEntity
    {
        var isBinaryOperation = Enum.TryParse(propertyOperation, out ExpressionType expressionType);

        if (isBinaryOperation)
        {
            var convertedValueForFiltering = convertedMember.InnerTypeForValueObjectOrCurrentTypeForPrimitive.ToConvertedExpression(predicate.Value);
            return Expression.MakeBinary(expressionType, convertedMember.ConvertedPropertyToFilterOn, convertedValueForFiltering);
        }

        if (propertyOperation is "Like")
        {
            if (likeProvider is null)
            {
                throw new ArgumentNullException(nameof(likeProvider), "When Like operation is required, than like provider must not be null");
            }

            return likeProvider.CreateLikeExpression(parameter, convertedMember.ConvertedPropertyToFilterOn, $"{predicate.Value}");
        }

        var method = StringType.GetMethod(propertyOperation, [StringType]);
        return Expression.Call(convertedMember.ConvertedPropertyToFilterOn, method!, Expression.Constant($"{predicate.Value}"));
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