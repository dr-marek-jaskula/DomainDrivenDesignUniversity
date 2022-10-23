using Shopway.Domain.Entities;
using System.Linq.Expressions;

namespace Shopway.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includes);

    void Create(Order order);

    void Update(Order order);

    void Remove(Order order);
}