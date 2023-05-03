﻿using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Products.Events;

internal sealed class LogHistoryWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILoggerAdapter<LogHistoryWhenProductCreatedDomainEventHandler> _logger;

    public LogHistoryWhenProductCreatedDomainEventHandler(IProductRepository productRepository, ILoggerAdapter<LogHistoryWhenProductCreatedDomainEventHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(ProductCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByIdAsync(domainEvent.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        _logger.LogInformation("Product with id {product.Id} was created", product.Id);
    }
}
