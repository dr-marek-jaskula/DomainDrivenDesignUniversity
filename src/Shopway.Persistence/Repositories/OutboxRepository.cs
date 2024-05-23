using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Infrastructure.Outbox;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using static Shopway.Infrastructure.Outbox.ExecutionStatus;

namespace Shopway.Persistence.Repositories;

internal sealed class OutboxRepository(ShopwayDbContext dbContext, TimeProvider timeProvider) : IOutboxRepository
{
    private readonly ShopwayDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private const int AmountOfProcessedMessages = 20;

    public async Task<OutboxMessage[]> GetOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ExecutionStatus == InProgress)
            .Where(m => m.NextProcessAttempt <= _timeProvider.GetUtcNow())
            .Take(AmountOfProcessedMessages)
            .ToArrayAsync(cancellationToken);
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
            .Where(entry => entry.Entity.DomainEvents.Count > 0)
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

    private OutboxMessage ToOutboxMessage(IDomainEvent domainEvent)
    {
        return new OutboxMessage
        {
            Id = domainEvent.Id,
            Type = domainEvent.GetType().Name,
            Content = domainEvent.Serialize(TypeNameHandling.All),
            OccurredOn = DateTimeOffset.UtcNow,
            ExecutionStatus = InProgress,
            NextProcessAttempt = _timeProvider.GetUtcNow().AddMinutes(OutboxMessage.InitialDelayInMinutes)
        };
    }
}
