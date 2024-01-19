using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.Features.Orders.Events;

internal sealed class DisplayMessageWhenOrderHeaderCreatedDomainEventHandler : IDomainEventHandler<OrderHeaderCreatedDomainEvent>
{
    public async Task Handle(OrderHeaderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("OrderHeader was created!");
    }
}
