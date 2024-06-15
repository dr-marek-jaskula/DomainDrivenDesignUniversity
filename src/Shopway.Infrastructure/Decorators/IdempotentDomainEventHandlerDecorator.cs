using Microsoft.Extensions.Logging;
using Shopway.Application.Abstractions;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Infrastructure.Outbox;

namespace Shopway.Infrastructure.Decoratos;

public sealed class IdempotentDomainEventHandlerDecorator<TDomainEvent>
(
    IDomainEventHandler<TDomainEvent> decorated,
    IOutboxRepository outboxRepository,
    ILogger<IdempotentDomainEventHandlerDecorator<TDomainEvent>> logger
)
    : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> _decorated = decorated;
    private readonly IOutboxRepository _outboxRepository = outboxRepository;
    private readonly ILogger<IdempotentDomainEventHandlerDecorator<TDomainEvent>> _logger = logger;

    public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        if (await _outboxRepository.IsConsumerAlreadyProcessed(domainEvent, consumer, cancellationToken))
        {
            return;
        }

        _logger.LogDomainEventHandlingByConsumer(typeof(TDomainEvent).Name, consumer);
        await _decorated.Handle(domainEvent, cancellationToken);

        await _outboxRepository
            .AddOutboxMessageConsumer(domainEvent, consumer);
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 1,
        EventName = $"{nameof(IdempotentDomainEventHandlerDecorator<DomainEvent>)}",
        Level = LogLevel.Information,
        Message = "Handling DomainEvent '{eventType}' by '{consumer}'",
        SkipEnabledCheck = true
    )]
    public static partial void LogDomainEventHandlingByConsumer(this ILogger logger, string eventType, string consumer);
}
