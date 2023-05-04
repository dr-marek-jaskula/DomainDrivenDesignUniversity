using Shopway.Persistence.Outbox;
using Shopway.Domain.Abstractions;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.Decoratos;

public sealed class IdempotentDomainEventHandlerDecorator<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> _decorated;
    private readonly IOutboxRepository _outboxRepository;

    public IdempotentDomainEventHandlerDecorator(IDomainEventHandler<TDomainEvent> decorated, IOutboxRepository outboxRepository)
    {
        _decorated = decorated;
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        if (await _outboxRepository.IsConsumerAlreadyProcessed(domainEvent, consumer, cancellationToken))
        {
            return;
        }

        await _decorated.Handle(domainEvent, cancellationToken);

        await _outboxRepository
            .AddOutboxMessageConsumer(domainEvent, consumer);
    }
}
