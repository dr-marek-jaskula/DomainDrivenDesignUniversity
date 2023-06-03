using Shopway.Domain.Common;
using Shopway.Domain.Utilities;
using System.Linq.Dynamic.Core;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.BaseTypes;

namespace Shopway.Persistence.Utilities;

public static class QueryableUtilities
{
    /// <summary>
    /// Get page items and total count
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">Entity id type</typeparam>
    /// <typeparam name="TResponse">Response type to map to</typeparam>
    /// <param name="queryable">Queryable</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple of  items and total count</returns>
    public static async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        this IQueryable<TResponse> queryable,
        IPage page,
        CancellationToken cancellationToken
    )
    {
        if (page is null)
        {
            throw new ArgumentNullException($"Page is null");
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var responses = await queryable
            .Page(page)
            .ToListAsync(cancellationToken);

        return (responses, totalCount);
    }

    internal static IQueryable<TResponse> Sort<TResponse>
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

    /// <summary>
    /// Using EF.Property
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityId"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="idPropertyName"></param>
    /// <returns></returns>
    public static async Task<bool> AnyAsync<TEntity, TEntityId>
    (
        this IQueryable<TEntity> queryable,
        TEntityId entityId,
        CancellationToken cancellationToken,
        string idPropertyName = IEntityId.Id
    )
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        return await queryable
           .Where(entity => EF.Property<TEntityId>(entity, idPropertyName).Equals(entityId))
           .AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Using dynamic linq
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityId"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> AnyAsyncUsingDynamicLinq<TEntity, TEntityId>
    (
        this IQueryable<TEntity> queryable,
        TEntityId entityId,
        CancellationToken cancellationToken,
        string idPropertyName = "Id"
    )
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        return await queryable
           .Where($"{idPropertyName} == \"{entityId.Value}\"")
           .AnyAsync(cancellationToken);
    }
}