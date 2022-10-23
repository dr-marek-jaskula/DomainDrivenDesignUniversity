using Shopway.Domain.Entities;
using System.Linq.Expressions;

namespace Shopway.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includes);
    
    Task<Customer?> GetAll(CancellationToken cancellationToken = default);

    void Add(Customer order);

    void Remove(Customer order);
}