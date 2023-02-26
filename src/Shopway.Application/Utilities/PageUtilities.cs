using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static async Task<PageResponse<TResponse>> ToPageResponse<TEntity, TResponse>
    (
        this IQueryable<TEntity> queryable,
        int pageSize,
        int pageNumber,
        Func<TEntity, TResponse> fromEntityToResponseMapper,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity
        where TResponse : class, IResponse
    {
        var numberOfSkippedItems = pageSize * (pageNumber - 1);

        var entities = await queryable
            .Skip(numberOfSkippedItems)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await queryable
            .CountAsync(cancellationToken);

        return CreatePageResponse(entities, totalCount, pageSize, numberOfSkippedItems, pageNumber, fromEntityToResponseMapper);
    }

    private static PageResponse<TRespone> CreatePageResponse<TEntity, TRespone>
    (
        IEnumerable<TEntity> entities, 
        int totalCount, 
        int pageSize,
        int numberOfSkippedItems,
        int pageNumber,
        Func<TEntity, TRespone> fromEntityToResponseMapper
    )
        where TEntity : class, IEntity
        where TRespone : class, IResponse
    {
        var response = entities
            .Select(fromEntityToResponseMapper)
            .ToList();

        var itemsFrom = numberOfSkippedItems + 1;
        var itemsTo = Math.Min(itemsFrom + pageSize - 1, totalCount);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PageResponse<TRespone>(response, totalCount, itemsFrom, itemsTo, totalPages, pageNumber);
    }
}