using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineCommand : IBatchCommand<BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse>
{
    public BatchUpsertOrderLineCommand(IList<BatchUpsertOrderLineRequest> requests)
    {
        Requests = requests;
    }

    public IList<BatchUpsertOrderLineRequest> Requests { get; set; }
    public OrderHeaderId OrderHeaderId { get; set; }

    public sealed record BatchUpsertOrderLineRequest
    (
        ProductId ProductId,
        int Amount,
        decimal? Discount
    )
        : IBatchRequest;
}