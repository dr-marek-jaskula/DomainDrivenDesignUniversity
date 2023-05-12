using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Products.Events;

internal sealed class DisplayMessageWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{

    public DisplayMessageWhenProductCreatedDomainEventHandler()
    {
    }

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("Product was created!");
    }
}
