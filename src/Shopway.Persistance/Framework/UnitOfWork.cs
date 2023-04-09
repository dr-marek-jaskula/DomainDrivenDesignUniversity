using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Outbox;

namespace Shopway.Persistence.Framework;

//UnitOfWork class to handler transactions
//Benefits:
//1. We do not want to pollute the application layer with entity framework
//2. We do not expose any implementation details when we inject IUnitOfWork interface
//a) since we use the repository pattern with UnitOfWork, the repositories do not contain SaveChanges method.
//This force us to call SaveChanges at the end of our business transactions from the UnitOfWork.
//So this removes the responsibility of SavingChanges from the repositories and moves it to the UnitOfWork
//b) since we use IUnitOfWork interface we can provide a mock for this interface
//3. Move the logic from the interceptors to the UnitOfWork
public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IUserContextService _userContext;
    private const string DefaultUsername = "Unknown";

    public UnitOfWork(TContext dbContext, IUserContextService userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public TContext Context => _dbContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Context
            .Database
            .BeginTransactionAsync(cancellationToken);
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Context
            .Database
            .CreateExecutionStrategy();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        ConvertDomainEventsToOutboxMessages();
        UpdateAuditableEntities();

        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = _dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents;
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTimeOffset.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject
                (
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )
            })
            .ToList();

        _dbContext
            .Set<OutboxMessage>()
            .AddRange(outboxMessages);
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditable>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditable>();

        foreach (EntityEntry<IAuditable> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOn).CurrentValue = DateTimeOffset.UtcNow;
                entityEntry.Property(a => a.CreatedBy).CurrentValue ??= _userContext.Username ?? DefaultUsername;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.UpdatedOn).CurrentValue = DateTimeOffset.UtcNow;
                entityEntry.Property(a => a.UpdatedBy).CurrentValue = _userContext.Username ?? DefaultUsername;
            }
        }
    }
}