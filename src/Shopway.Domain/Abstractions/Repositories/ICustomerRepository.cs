using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(PersonId id, CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdWithIncludesAsync(PersonId id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includes);

    Task<Customer?> GetAll(CancellationToken cancellationToken = default);

    void Add(Customer order);

    void Remove(Customer order);
}