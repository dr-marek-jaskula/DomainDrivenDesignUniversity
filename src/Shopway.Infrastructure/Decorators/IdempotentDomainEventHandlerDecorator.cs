using Shopway.Application.Abstractions;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Persistence.Outbox;

namespace Shopway.Infrastructure.Decoratos;

public sealed class IdempotentDomainEventHandlerDecorator<TDomainEvent>(IDomainEventHandler<TDomainEvent> decorated, IOutboxRepository outboxRepository)
    : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> _decorated = decorated;
    private readonly IOutboxRepository _outboxRepository = outboxRepository;

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
