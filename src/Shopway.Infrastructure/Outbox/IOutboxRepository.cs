using Shopway.Domain.Abstractions;

namespace Shopway.Persistence.Outbox;

public interface IOutboxRepository
{
    Task<IList<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken);

    Task<bool> IsConsumerAlreadyProcessed(IDomainEvent domainEvent, string consumer, CancellationToken cancellationToken);

    Task AddOutboxMessageConsumer(IDomainEvent domainEvent, string consumer);

    void PersistOutboxMessagesFromDomainEvents();
}