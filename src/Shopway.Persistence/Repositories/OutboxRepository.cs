using Newtonsoft.Json;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Outbox;

namespace Shopway.Persistence.Repositories;

public sealed class OutboxRepository(ShopwayDbContext dbContext) : RepositoryBase(dbContext), IOutboxRepository
{
    private const int AmountOfProcessedMessages = 20;

    public async Task<IList<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOn == null)
            .Take(AmountOfProcessedMessages)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsConsumerAlreadyProcessed(IDomainEvent domainEvent, string consumer, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<OutboxMessageConsumer>()
            .Where(outboxMessageConsumer => outboxMessageConsumer.Id == domainEvent.Id)
            .Where(outboxMessageConsumer => outboxMessageConsumer.Name == consumer)
            .AnyAsync(cancellationToken);
    }

    public async Task AddOutboxMessageConsumer(IDomainEvent domainEvent, string consumer)
    {
        _dbContext
            .Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = domainEvent.Id,
                Name = consumer
            });

        //At this stage we do not want to cancel the operation, so we use the CancellationToken.None.
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public void PersistOutboxMessagesFromDomainEvents()
    {
        var outboxMessages = _dbContext
            .ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents.ToArray();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(ToOutboxMessage)
            .ToList();

        _dbContext
            .Set<OutboxMessage>()
            .AddRange(outboxMessages);
    }

    private static OutboxMessage ToOutboxMessage(IDomainEvent domainEvent)
    {
        return new OutboxMessage
        {
            Id = Ulid.NewUlid(),
            Type = domainEvent.GetType().Name,
            Content = domainEvent.Serialize(TypeNameHandling.All),
            OccurredOn = DateTimeOffset.UtcNow
        };
    }
}