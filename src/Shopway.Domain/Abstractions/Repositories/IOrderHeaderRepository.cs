using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IOrderHeaderRepository
{
    Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken);

    Task<OrderHeader> GetByIdWithOrderLineAsync(OrderHeaderId id, OrderLineId orderLineId, CancellationToken cancellationToken);

    Task<OrderHeader> GetByIdWithIncludesAsync(OrderHeaderId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object>>[] includes);

    void Create(OrderHeader order);

    void Update(OrderHeader order);

    void Remove(OrderHeader order);
}