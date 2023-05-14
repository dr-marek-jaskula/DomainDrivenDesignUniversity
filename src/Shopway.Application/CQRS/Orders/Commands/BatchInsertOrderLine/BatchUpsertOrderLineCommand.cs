using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineCommand : IBatchCommand<BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse>
{
    public OrderHeaderId OrderHeaderId { get; init; }

    public BatchUpsertOrderLineCommand(OrderHeaderId orderHeaderId, IList<BatchUpsertOrderLineRequest> requests)
    {
        OrderHeaderId = orderHeaderId;
        Requests = requests;
    }

    public IList<BatchUpsertOrderLineRequest> Requests { get; set; }

    public sealed record BatchUpsertOrderLineRequest
    (
        OrderLineKey OrderLineKey,
        int Amount,
        decimal? Discount
    )
        : IBatchRequest;
}