using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Infrastructure.Outbox;

public interface IOutboxRepository
{
    Task<OutboxMessage[]> GetOutboxMessagesAsync(CancellationToken cancellationToken);

    Task<bool> IsConsumerAlreadyProcessed(IDomainEvent domainEvent, string consumer, CancellationToken cancellationToken);

    Task AddOutboxMessageConsumer(IDomainEvent domainEvent, string consumer);

    void PersistOutboxMessagesFromDomainEvents();
}