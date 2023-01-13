using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdWithIncludesAsync(OrderId id, CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[] includes);

    void Create(Order order);

    void Update(Order order);

    void Remove(Order order);
}