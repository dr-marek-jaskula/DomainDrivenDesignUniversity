using Microsoft.Extensions.Logging;
using Shopway.Application.Abstractions;
using Shopway.Domain.Products;
using Shopway.Domain.Products.Events;

namespace Shopway.Application.Features.Products.Events;

internal sealed class LogHistoryWhenProductCreatedDomainEventHandler(ILogger<LogHistoryWhenProductCreatedDomainEventHandler> logger)
    : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly ILogger<LogHistoryWhenProductCreatedDomainEventHandler> _logger = logger;

    public async Task Handle(ProductCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        _logger.LogProductCreation(domainEvent.ProductId);
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 5,
        EventName = $"{nameof(LogHistoryWhenProductCreatedDomainEventHandler)}",
        Level = LogLevel.Information,
        Message = "Product with id {productId} was created",
        SkipEnabledCheck = false
    )]
    public static partial void LogProductCreation(this ILogger logger, ProductId productId);
}
