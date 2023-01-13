using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Products.Events;

internal sealed class DisplayMessageWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IProductRepository _productRepository;

    public DisplayMessageWhenProductCreatedDomainEventHandler(IProductRepository productRepository)
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
