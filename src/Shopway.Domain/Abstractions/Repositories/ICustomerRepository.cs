using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken);

    Task<Customer?> GetByIdWithIncludesAsync(CustomerId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object?>>[] includes);

    Task<Customer?> GetAll(CancellationToken cancellationToken);

    void Add(Customer order);

    void Remove(Customer order);
}