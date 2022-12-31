using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Repositories;

namespace Shopway.Application.CQRS.Orders.Events;

internal sealed class DisplayMessageWhenOrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;

    public DisplayMessageWhenOrderCreatedDomainEventHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
        {
            return;
        }

        Console.WriteLine("Order was created!");
    }
}
