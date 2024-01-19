using Shopway.Application.Abstractions;
using Shopway.Domain.Products.Events;

namespace Shopway.Application.Features.Products.Events;

internal sealed class DisplayMessageWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("Product was created!");
    }
}
