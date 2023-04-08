using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(OrderId id, CancellationToken cancellationToken);

    Task<Order> GetByIdWithIncludesAsync(OrderId id, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes);

    void Create(Order order);

    void Update(Order order);

    void Remove(Order order);
}