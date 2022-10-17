using Shopway.Domain.Entities;
using System.Linq.Expressions;

namespace Shopway.Domain.Repositories;

internal interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includes);

    void Add(Order order);

    void Remove(Order order);
}