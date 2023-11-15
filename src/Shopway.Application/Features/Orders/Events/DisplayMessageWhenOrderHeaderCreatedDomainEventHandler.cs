using Shopway.Domain.DomainEvents;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Events;

internal sealed class DisplayMessageWhenOrderHeaderCreatedDomainEventHandler : IDomainEventHandler<OrderHeaderCreatedDomainEvent>
{
    public async Task Handle(OrderHeaderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("OrderHeader was created!");
    }
}
