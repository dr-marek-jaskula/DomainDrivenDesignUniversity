using Shopway.Domain.Common;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Shopway.Persistence.Utilities;

internal static class QueryableUtilities
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
    internal static async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
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

    internal static IQueryable<TResponse> Apply<TResponse>
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
}