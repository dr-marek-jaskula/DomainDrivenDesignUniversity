using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Shopway.Persistence.Utilities;

internal static class QueryableUtilities
{
    /// <summary>
    /// Add pagination from specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="queryable">_dbContext.Set<TResponse>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    internal static IQueryable<TResponse> Page<TEntity, TEntityId, TResponse>(this IQueryable<TResponse> queryable, SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        if (specification.Page is null)
        {
            throw new NotImplementedException("Page is null");
        }

        return queryable.Page(specification.Page.PageSize, specification.Page.PageNumber);
    }

    /// <summary>
    /// Get page items and total count
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">Entity id type</typeparam>
    /// <typeparam name="TResponse">Response type to map to</typeparam>
    /// <param name="specification">Mapping specification</param>
    /// <param name="queryable">Queryable</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple of  items and total count</returns>
    internal static async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TEntity, TEntityId, TResponse>
    (
        this IQueryable<TResponse> queryable,
        SpecificationWithMappingBase<TEntity, TEntityId, TResponse> specification,
        CancellationToken cancellationToken
    )
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        var totalCount = await queryable.CountAsync(cancellationToken);

        var responses = await queryable
            .Page(specification)
            .ToListAsync(cancellationToken);

        return (responses, totalCount);
    }
}