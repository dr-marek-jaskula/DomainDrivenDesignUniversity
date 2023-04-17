using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Utilities;
using System.Linq.Expressions;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static async Task<PageResponse<TResponse>> ToPageResponse<TEntity, TResponse>
    (
        this IQueryable<TEntity> queryable,
        int pageSize,
        int pageNumber,
        Expression<Func<TEntity, TResponse>> fromEntityToResponseMapper,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity
        where TResponse : class, IResponse
    {
        var response = await queryable
            .Page(pageSize, pageNumber)
            .Select(fromEntityToResponseMapper)
            .ToListAsync(cancellationToken);

        var totalCount = await queryable
            .CountAsync(cancellationToken);

        return new PageResponse<TResponse>(response, totalCount, pageSize, pageNumber);
    }
}