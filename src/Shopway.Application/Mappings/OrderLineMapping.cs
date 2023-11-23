using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using Shopway.Application.Features;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;
using Shopway.Application.Features.Orders.Commands.UpdateOrderLine;
using Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

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
            orderLine.CalculateLineCost(),
            orderLine.ProductSummary.ToSummaryResponse()
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
        return OrderLineKey.Create(orderLine.ProductSummary.ProductId);
    }
}