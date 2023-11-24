using System.Data;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Persistence.Utilities;

public static class QueryableUtilities
{
    private const int AdditionalRecordForCursor = 1;

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
            throw new ArgumentNullException($"Page is null");
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
        int take,
        CancellationToken cancellationToken
    )
        where TResponse : class, IHasCursor
    {
        var responsesWithCursor = await queryable
            .Take(take + AdditionalRecordForCursor)
            .ToListAsync(cancellationToken);

        var cursor = Ulid.Empty;
        if (responsesWithCursor.Count > take)
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
}