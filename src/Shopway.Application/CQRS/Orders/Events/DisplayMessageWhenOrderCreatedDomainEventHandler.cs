using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Orders.Events;

internal sealed class DisplayMessageWhenOrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
{
    public DisplayMessageWhenOrderCreatedDomainEventHandler()
    {
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("Order was created!");
    }
}
