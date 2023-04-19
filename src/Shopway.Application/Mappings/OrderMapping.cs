using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Commands.UpdateOrder;
using Shopway.Application.CQRS.Orders.Queries;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mappings;

public static class OrderMapping
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        (
            Id: order.Id.Value,
            Amount: order.Amount,
            Status: order.Status,
            Product: order.Product,
            Payment: order.Payment,
            Customer: order.Customer
        );
    }

    public static UpdateOrderResponse ToUpdateResponse(this Order orderToUpdate)
    {
        return new UpdateOrderResponse(orderToUpdate.Id.Value);
    }

    public static CreateOrderResponse ToCreateResponse(this Order orderToCreate)
    {
        return new CreateOrderResponse(orderToCreate.Id.Value);
    }
}