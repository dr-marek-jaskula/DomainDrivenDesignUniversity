using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Orders;
using Shopway.Application.CQRS.Orders.Commands.AddOrderLine;
using Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;
using Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;
using Shopway.Application.CQRS;
using Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;
using Shopway.Domain.EntityBusinessKeys;
using static Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine.BatchInsertOrderLineCommand;

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

    public static RemoveOrderLineResponse ToRemoveResponse(this OrderLine orderLineToRemove)
    {
        return new RemoveOrderLineResponse(orderLineToRemove.Id.Value);
    }

    public static UpdateOrderLineResponse ToUpdateResponse(this OrderLine orderLineToUpdate)
    {
        return new UpdateOrderLineResponse(orderLineToUpdate.Id.Value);
    }

    public static BatchInsertOrderLineResponse ToBatchInsertResponse(this IList<BatchResponseEntry> batchResponseEntries)
    {
        return new BatchInsertOrderLineResponse(batchResponseEntries);
    }

    public static OrderLineKey MapFromRequestToOrderLineKey(BatchInsertOrderLineRequest orderLineBatchRequest)
    {
        return orderLineBatchRequest.OrderLineKey;
    }
}