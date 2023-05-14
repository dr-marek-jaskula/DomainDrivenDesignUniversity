using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Orders;
using Shopway.Application.CQRS.Orders.Commands.AddOrderLine;

namespace Shopway.Application.Mappings;

public static class OrderLineMapping
{
    public static OrderLineResponse ToResponse(this OrderLine orderLine)
    {
        return new OrderLineResponse
        (
            orderLine.Id.Value,
            orderLine.Amount.Value,
            orderLine.LineDiscount.Value,
            orderLine.Product.ProductName.Value,
            orderLine.Product.Revision.Value,
            orderLine.Product.Price.Value,
            orderLine.CalculateLineCost()
        );
    }
    
    public static IReadOnlyCollection<OrderLineResponse> ToResponses(this IReadOnlyCollection<OrderLine> orderLines)
    {
        return orderLines
            .Select(ToResponse)
            .ToList()
            .AsReadOnly();
    }

    public static AddOrderLineResponse ToAddResponse(this OrderLine orderLineToAdd)
    {
        return new AddOrderLineResponse(orderLineToAdd.Id.Value);
    }
}