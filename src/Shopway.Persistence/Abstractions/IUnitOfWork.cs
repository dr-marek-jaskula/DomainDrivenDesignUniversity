using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shopway.Persistence.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    IExecutionStrategy CreateExecutionStrategy();
}

public interface IUnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    TContext Context { get; }
}
