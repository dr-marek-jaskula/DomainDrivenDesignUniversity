using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static async Task<PageResponse<TResponse>> ToPage<TEntity, TResponse>
    (
        this IQueryable<TEntity> queryable,
        int pageSize,
        int pageNumber,
        Func<TEntity, TResponse> toResponse,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity
        where TResponse : class, IResponse
    {
        var entities = await queryable
            .ToListAsync(cancellationToken);

        var totalCount = await queryable
            .CountAsync(cancellationToken);

        return Page(entities, totalCount, pageSize, pageNumber, toResponse);
    }

    private static PageResponse<TRespone> Page<TEntity, TRespone>
    (
        IEnumerable<TEntity> entities, 
        int totalCount, 
        int pageSize,
        int pageNumber,
        Func<TEntity, TRespone> toResponse
    )
        where TEntity : class, IEntity
        where TRespone : class, IResponse
    {
        var response = entities
            .Select(toResponse)
            .ToList();

        return new PageResponse<TRespone>(response, totalCount, pageSize, pageNumber);
    }
}