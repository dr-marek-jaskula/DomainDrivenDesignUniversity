using Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.CQRS.Orders.Queries;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mappings;

public static class OrderMapping
{
    public static OrderHeaderResponse ToResponse(this OrderHeader orderHeader)
    {
        return new OrderHeaderResponse
        (
            Id: orderHeader.Id.Value,
            Status: orderHeader.Status,
            Payment: orderHeader.Payment,
            User: orderHeader.User
        );
    }

    public static CreateOrderHeaderResponse ToCreateResponse(this OrderHeader orderToCreate)
    {
        return new CreateOrderHeaderResponse(orderToCreate.Id.Value);
    }
}