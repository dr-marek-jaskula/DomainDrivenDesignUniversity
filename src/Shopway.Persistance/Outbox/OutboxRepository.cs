using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Outbox;

public sealed class OutboxRepository : RepositoryBase, IOutboxRepository
{
    private const int AmountOfProcessedMessages = 20;

    public OutboxRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

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
            .AnyAsync(outboxMessageConsumer =>
                outboxMessageConsumer.Id == domainEvent.Id && outboxMessageConsumer.Name == consumer,
                cancellationToken);
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

    public void ConvertDomainEventsToOutboxMessages()
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
        };
    }
}