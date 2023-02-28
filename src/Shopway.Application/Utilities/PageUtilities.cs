using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Utilities;

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
        var entities = await queryable
            .Page(pageSize, pageNumber)
            .ToListAsync(cancellationToken);

        var totalCount = await queryable
            .CountAsync(cancellationToken);

        var response = entities
            .Select(fromEntityToResponseMapper)
            .ToList();

        return new PageResponse<TResponse>(response, totalCount, pageSize, pageNumber);
    }
}