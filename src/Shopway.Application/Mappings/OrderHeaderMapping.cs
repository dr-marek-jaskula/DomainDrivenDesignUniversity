using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Orders.Queries;
using Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;

namespace Shopway.Application.Mappings;

public static class OrderHeaderMapping
{
    public static OrderHeaderResponse ToResponse(this OrderHeader orderHeader)
    {
        return new OrderHeaderResponse
        (
            orderHeader.Id.Value,
            orderHeader.Status,
            orderHeader.Payment.Status,
            orderHeader.CalculateTotalPrice(),
            orderHeader.TotalDiscount.Value,
            orderHeader.OrderLines.ToResponses()
        );
    }

    public static CreateOrderHeaderResponse ToCreateResponse(this OrderHeader orderHeaderToCreate)
    {
        return new CreateOrderHeaderResponse(orderHeaderToCreate.Id.Value);
    }

    public static ChangeOrderHeaderStatusResponse ToChangeStatusResponse(this OrderHeader orderHeader)
    {
        return new ChangeOrderHeaderStatusResponse(orderHeader.Id.Value);
    }
}