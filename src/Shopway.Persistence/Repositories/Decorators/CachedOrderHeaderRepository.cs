using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Orders;
using Shopway.Domain.Users;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Persistence.Repositories.Decorators;

internal sealed class CachedOrderHeaderRepository(IOrderHeaderRepository decorated, IFusionCache fusionCache, ShopwayDbContext dbContext)
    : IOrderHeaderRepository
{
    private readonly IOrderHeaderRepository _decorated = decorated;
    private readonly IFusionCache _fusionCache = fusionCache;
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken)
    {
        var order = await _fusionCache.GetOrSetAsync
        (
            id.ToCacheKey(),
            cancellationToken => _decorated.GetByIdAsync(id, cancellationToken)!,
            TimeSpan.FromSeconds(30),
            cancellationToken
        );

        return _dbContext.AttachToChangeTrackerWhenTrackingBehaviorIsDifferentFromNoTracking(order);
    }

    public Task<OrderHeader> GetByIdWithIncludesAsync(OrderHeaderId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object>>[] includes)
    {
        return _decorated.GetByIdWithIncludesAsync(id, cancellationToken, includes);
    }

    public async Task<bool> IsOrderHeaderCreatedByUser(OrderHeaderId id, UserId userId, CancellationToken cancellationToken)
    {
        return await _decorated.IsOrderHeaderCreatedByUser(id, userId, cancellationToken);
    }

    public async Task<OrderHeader?> GetByPaymentSessionIdAsync(string sessionId, CancellationToken cancellationToken)
    {
        return await _decorated.GetByPaymentSessionIdAsync(sessionId, cancellationToken);
    }

    public void Create(OrderHeader order)
    {
        _decorated.Create(order);
    }

    public void Remove(OrderHeader order)
    {
        _decorated.Remove(order);
    }

    public void Update(OrderHeader order)
    {
        _decorated.Update(order);
        _fusionCache.Update<OrderHeader, OrderHeaderId>(order);
    }

    public async Task<OrderHeader> GetByIdWithOrderLineAsync(OrderHeaderId id, OrderLineId orderLineId, CancellationToken cancellationToken)
    {
        return await _decorated.GetByIdWithOrderLineAsync(id, orderLineId, cancellationToken);
    }

    public async Task<(List<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        List<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            filter: filter,
            likes: likes,
            sort: sort,
            mapping: mapping,
            mappingExpression: mappingExpression,
            buildIncludes: buildIncludes
        );
    }

    public async Task<(List<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        List<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
        where TResponse : class, IHasCursor
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            filter: filter,
            likes: likes,
            sort: sort,
            mapping: mapping,
            mappingExpression: mappingExpression,
            buildIncludes: buildIncludes
        );
    }

    public async Task<OrderHeader> QueryByIdAsync
    (
        OrderHeaderId orderHeaderId,
        CancellationToken cancellationToken,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
    {
        return await _decorated.QueryByIdAsync
        (
            orderHeaderId,
            cancellationToken,
            buildIncludes
        );
    }

    public async Task<TResponse> QueryByIdAsync<TResponse>
    (
        OrderHeaderId orderHeaderId,
        CancellationToken cancellationToken,
        IMapping<OrderHeader, TResponse>? mapping
    )
        where TResponse : class
    {
        return await _decorated.QueryByIdAsync
        (
            orderHeaderId,
            cancellationToken,
            mapping
        );
    }
}
