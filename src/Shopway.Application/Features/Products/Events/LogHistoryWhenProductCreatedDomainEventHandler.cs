using Shopway.Domain.DomainEvents;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Events;

internal sealed class LogHistoryWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly ILoggerAdapter<LogHistoryWhenProductCreatedDomainEventHandler> _logger;

    public LogHistoryWhenProductCreatedDomainEventHandler(ILoggerAdapter<LogHistoryWhenProductCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        _logger.LogInformation("Product with id {product.Id} was created", domainEvent.ProductId);
    }
}
