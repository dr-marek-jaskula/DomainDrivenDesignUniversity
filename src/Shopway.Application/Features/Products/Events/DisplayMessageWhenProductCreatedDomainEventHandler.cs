using Shopway.Application.Abstractions;
using Shopway.Domain.Products.Events;

namespace Shopway.Application.Features.Products.Events;

internal sealed class DisplayMessageWhenProductCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task Handle(ProductCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("Product was created!");
    }
}
