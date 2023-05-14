using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine.BatchInsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;

public sealed record BatchInsertOrderLineCommand : IBatchCommand<BatchInsertOrderLineRequest, BatchInsertOrderLineResponse>
{
    public OrderHeaderId OrderHeaderId { get; init; }

    public BatchInsertOrderLineCommand(OrderHeaderId orderHeaderId, IList<BatchInsertOrderLineRequest> requests)
    {
        OrderHeaderId = orderHeaderId;
        Requests = requests;
    }

    public IList<BatchInsertOrderLineRequest> Requests { get; set; }

    public sealed record BatchInsertOrderLineRequest
    (
        OrderLineKey OrderLineKey,
        ProductId ProductId,
        int Amount,
        decimal? Discount
    )
        : IBatchRequest;
}