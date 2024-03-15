using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Exceptions;
using Shopway.Domain.Common.Utilities;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using static Shopway.Domain.Constants.Constants.Type;

namespace Shopway.Persistence.Utilities;

public static class QueryableUtilities
{
    private const int AdditionalRecordForCursor = 1;

    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), StringType, StringType])
        ?? throw new ArgumentNullException(nameof(DbFunctionsExtensions.Like), "The EF.Functions.Like not found");

    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))
        ?? throw new ArgumentNullException(nameof(EF.Functions), "The EF.Functions not found"));

    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
        .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
        .Single(mi => mi.GetGenericArguments().Length is 2
            && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
            && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(methodInfo => methodInfo.GetGenericArguments().Length is 3
                && methodInfo.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && methodInfo.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && methodInfo.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Where(methodInfo => methodInfo.GetGenericArguments().Length is 3)
            .Single(methodInfo =>
            {
                var typeInfo = methodInfo.GetParameters()[0].ParameterType.GenericTypeArguments[1];
                return typeInfo.IsGenericType
                        && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        && methodInfo.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                        && methodInfo.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>);
            });

    /// <summary>
    /// Get page items and total count
    /// </summary>
    /// <typeparam name="TResponse">Response type to map to</typeparam>
    /// <param name="queryable">Queryable</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple of  items and total count</returns>
    public static async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        this IQueryable<TResponse> queryable,
        IOffsetPage page,
        CancellationToken cancellationToken
    )
    {
        if (page is null)
        {
            throw new ArgumentNullException(nameof(page), "Page is null");
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var responses = await queryable
            .Page(page)
            .ToListAsync(cancellationToken);

        return (responses, totalCount);
    }

    /// <summary>
    /// Get page items and the next cursor
    /// </summary>
    /// <typeparam name="TResponse">Response type to map to</typeparam>
    /// <param name="queryable">Queryable</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple of  items and next cursor. If the last record was reach, the Ulid.Empty is returned as next cursor</returns>
    public static async Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        this IQueryable<TResponse> queryable,
        ICursorPage page,
        CancellationToken cancellationToken
    )
        where TResponse : class, IHasCursor
    {
        var responsesWithCursor = await queryable
            .Take(page.PageSize + AdditionalRecordForCursor)
            .ToListAsync(cancellationToken);

        var cursor = Ulid.Empty;
        if (responsesWithCursor.Count > page.PageSize)
        {
            cursor = responsesWithCursor.Last().Id;
            responsesWithCursor = responsesWithCursor.SkipLast(1).ToList();
        }

        return (responsesWithCursor, cursor);
    }

    public static async Task<bool> AnyAsync<TEntity, TEntityId>
    (
        this IQueryable<TEntity> queryable,
        TEntityId entityId,
        CancellationToken cancellationToken
    )
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return await queryable
           .Where(entity => entity.Id.Equals(entityId))
           .AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Using EF.Property
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityId"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> AnyAsyncUsingEFProperty<TEntity, TEntityId>
    (
        this IQueryable<TEntity> queryable,
        TEntityId entityId,
        CancellationToken cancellationToken
    )
        where TEntity : IEntity
        where TEntityId : struct, IEntityId
    {
        return await queryable
           .Where(entity => EF.Property<TEntityId>(entity, IEntityId.Id).Equals(entityId))
           .AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Using dynamic linq
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> AnyAsyncUsingDynamicLinq<TEntity>
    (
        this IQueryable<TEntity> queryable,
        IEntityId entityId,
        CancellationToken cancellationToken
    )
        where TEntity : IEntity
    {
        return await queryable
           .Where($"{IEntityId.Id} == \"{entityId.Value}\"")
           .AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Filters <paramref name="queryable"/> by applying an 'SQL LIKE' operation. Works for string properties and ValueObject with Value. 
    /// Likes are chain in "AndAlso" manner.
    /// </summary>
    public static IQueryable<TEntity> Like<TEntity>(this IQueryable<TEntity> queryable, IList<LikeEntry<TEntity>> likeEntries)
        where TEntity : class, IEntity
    {
        if (likeEntries.Count is 0)
        {
            return queryable;
        }

        Expression? expression = null;
        var parameter = Expression.Parameter(typeof(TEntity));

        foreach (var likeEntry in likeEntries)
        {
            var propertyName = likeEntry.Property.GetPropertyName();
            var likeExpression = CreateLikeExpression(parameter, propertyName, likeEntry.LikeTerm);

            expression = expression is null 
                ? likeExpression 
                : Expression.AndAlso(expression, likeExpression);
        }

        return expression is null
            ? queryable
            : queryable.Where(Expression.Lambda<Func<TEntity, bool>>(expression, parameter));
    }

    /// <summary>
    /// Filters <paramref name="queryable"/> by applying an 'SQL LIKE' operation. Works for string properties and ValueObject with Value. 
    /// </summary>
    public static Expression CreateLikeExpression(ParameterExpression parameter, Expression property, string likeTerm)
    {
        if (likeTerm.IsNullOrEmptyOrWhiteSpace())
        {
            throw new InvalidLikePatternException($"search pattern is null or empty for {property}.");
        }

        Expression convertedPropertyToFilterOn = property.Type == StringType
            ? property
            : property.ConvertToObjectAndThenToGivenType(StringType);

        var lambdaExpression = Expression.Lambda(convertedPropertyToFilterOn!, parameter);

        if (lambdaExpression is null)
        {
            throw new InvalidExpressionException();
        }

        var searchTermAsExpression = ((Expression<Func<string>>)(() => likeTerm)).Body;

        return Expression.Call
        (
            null,
            _likeMethodInfo,
            _functions,
            lambdaExpression.Body,
            searchTermAsExpression
        );
    }

    /// <summary>
    /// Filters <paramref name="queryable"/> by applying an 'SQL LIKE' operation. Works for string properties and ValueObject with Value. 
    /// </summary>
    public static Expression CreateLikeExpression(ParameterExpression parameter, string property, string likeTerm)
    {
        var memberExpression = parameter.ToMemberExpression(property);
        return CreateLikeExpression(parameter, memberExpression, likeTerm);
    }

    public static IQueryable<TEntity> AddInclude<TEntity, TEntityId>(this IQueryable<TEntity> query, IncludeEntry<TEntity> includeEntry)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        var newQueryable = _includeMethodInfo
            .MakeGenericMethod(includeEntry.EntityType, includeEntry.PropertyType)
            .Invoke(null, [ query, includeEntry.Property ]);

        if (newQueryable is null)
        {
            throw new ArgumentNullException(nameof(newQueryable));
        }

        return (IQueryable<TEntity>)newQueryable;
    }

    public static IQueryable<TEntity> AddThenInclude<TEntity, TEntityId>(this IQueryable<TEntity> queryable, IncludeEntry<TEntity> includeEntry)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        if (includeEntry.PreviousPropertyType is null)
        {
            throw new ArgumentNullException(nameof(includeEntry.PreviousPropertyType));
        }

        var thenIncludeMethodInfo = includeEntry.PreviousPropertyType.IsGenericEnumerable(out var previousPropertyType)
            ? _thenIncludeAfterEnumerableMethodInfo
            : _thenIncludeAfterReferenceMethodInfo;

         var newQueryable = thenIncludeMethodInfo
            .MakeGenericMethod(includeEntry.EntityType, previousPropertyType, includeEntry.PropertyType)
            .Invoke(null, [ queryable, includeEntry.Property ]);

        if (newQueryable is null)
        {
            throw new ArgumentNullException(nameof(newQueryable));
        }

        return (IQueryable<TEntity>)newQueryable;
    }
}