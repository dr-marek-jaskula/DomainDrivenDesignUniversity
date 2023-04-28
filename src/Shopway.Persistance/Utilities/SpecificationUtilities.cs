using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Utilities;

internal static class SpecificationUtilities
{
    /// <summary>
    /// Cast specification to mapping specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    internal static SpecificationWithMappingBase<TEntity, TEntityId, TResponse> AsMappingSpecification<TEntity, TEntityId, TResponse>(this SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        return (SpecificationWithMappingBase<TEntity, TEntityId, TResponse>)specification;
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
    internal static async Task<(IList<TResponse> Responses, int TotalCount)> Page<TEntity, TEntityId, TResponse>
    (
        this SpecificationWithMappingBase<TEntity, TEntityId, TResponse> specification,
        IQueryable<TResponse> queryable,
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