using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Orders.Events;

internal sealed class DisplayMessageWhenOrderHeaderCreatedDomainEventHandler : IDomainEventHandler<OrderHeaderCreatedDomainEvent>
{
    public DisplayMessageWhenOrderHeaderCreatedDomainEventHandler()
    {
    }

    public async Task Handle(OrderHeaderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("OrderHeader was created!");
    }
}
