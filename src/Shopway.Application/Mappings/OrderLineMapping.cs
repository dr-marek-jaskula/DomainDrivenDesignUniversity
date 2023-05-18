using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Orders;
using Shopway.Application.CQRS.Orders.Commands.AddOrderLine;
using Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;
using Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;
using Shopway.Application.CQRS;
using Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;
using Shopway.Domain.EntityKeys;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

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

    public static BatchUpsertOrderLineResponse ToBatchInsertResponse(this IList<BatchResponseEntry> batchResponseEntries)
    {
        return new BatchUpsertOrderLineResponse(batchResponseEntries);
    }

    public static OrderLineKey MapFromRequestToOrderLineKey(BatchUpsertOrderLineRequest orderLineBatchRequest)
    {
        return OrderLineKey.Create(orderLineBatchRequest.ProductId);
    }

    public static OrderLineKey ToOrderLineKey(this BatchUpsertOrderLineRequest orderLineBatchRequest)
    {
        return OrderLineKey.Create(orderLineBatchRequest.ProductId);
    }

    public static OrderLineKey ToOrderLineKey(this OrderLine orderLine)
    {
        return OrderLineKey.Create(orderLine.ProductId);
    }
}