using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Repositories;

namespace Shopway.Application.Orders.Events;

internal sealed class DisplyMessageWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IProductRepository _productRepository;

    public DisplyMessageWhenProductCreatedDomainEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        Console.WriteLine("Product was created!");
    }
}
