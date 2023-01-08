using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shopway.Persistence.Framework;

namespace Shopway.Domain.Repositories;

public interface IUnitOfWork
{
    public ShopwayDbContext Context { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    IExecutionStrategy CreateExecutionStrategy();
}
