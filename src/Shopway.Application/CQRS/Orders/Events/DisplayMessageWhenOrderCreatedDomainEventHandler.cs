using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Orders.Events;

internal sealed class DisplayMessageWhenOrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;

    public DisplayMessageWhenOrderCreatedDomainEventHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = await _orderRepository
            .GetByIdAsync(domainEvent.OrderId, cancellationToken);

        if (order is null)
        {
            return;
        }

        Console.WriteLine("Order was created!");
    }
}
