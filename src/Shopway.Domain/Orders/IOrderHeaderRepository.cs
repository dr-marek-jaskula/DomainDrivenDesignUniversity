using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Users;
using System.Linq.Expressions;

namespace Shopway.Domain.Orders;

public interface IOrderHeaderRepository
{
    Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken);

    Task<OrderHeader> GetByIdWithOrderLineAsync(OrderHeaderId id, OrderLineId orderLineId, CancellationToken cancellationToken);

    Task<OrderHeader> GetByIdWithIncludesAsync(OrderHeaderId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object>>[] includes);

    Task<bool> IsOrderHeaderCreatedByUser(OrderHeaderId id, UserId userId, CancellationToken cancellationToken);

    Task<OrderHeader?> GetByPaymentSessionIdAsync(string sessionId, CancellationToken cancellationToken);

    void Create(OrderHeader order);

    void Update(OrderHeader order);

    void Remove(OrderHeader order);

    Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        IList<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    );

    Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        IList<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
        where TResponse : class, IHasCursor;
}