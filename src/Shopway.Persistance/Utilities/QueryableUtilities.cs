using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Utilities;

public static class QueryableUtilities
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
    public static IQueryable<TResponse> Page<TEntity, TEntityId, TResponse>(this IQueryable<TResponse> queryable, SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        if (specification.Page is null)
        {
            throw new NotImplementedException("Page is null");
        }

        return queryable.Page(specification.Page.PageSize, specification.Page.PageNumber);
    }
}