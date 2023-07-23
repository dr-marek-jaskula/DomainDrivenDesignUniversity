using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shopway.Persistence.Abstractions;

public interface IUnitOfWork<TContext>
    where TContext : DbContext
{
    TContext Context { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    IExecutionStrategy CreateExecutionStrategy();
}
