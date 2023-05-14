using Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.CQRS.Orders.Queries;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mappings;

public static class OrderHeaderMapping
{
    public static OrderHeaderResponse ToResponse(this OrderHeader orderHeader)
    {
        return new OrderHeaderResponse
        (
            orderHeader.Id.Value,
            orderHeader.Status,
            orderHeader.Payment.CalculateTotalPayment(),
            orderHeader.Payment.TotalDiscount.Value,
            orderHeader.OrderLines.ToResponses()
        );
    }

    public static CreateOrderHeaderResponse ToCreateResponse(this OrderHeader orderToCreate)
    {
        return new CreateOrderHeaderResponse(orderToCreate.Id.Value);
    }
}