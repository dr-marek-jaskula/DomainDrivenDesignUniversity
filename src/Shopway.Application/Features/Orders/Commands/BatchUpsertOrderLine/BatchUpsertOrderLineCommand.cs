using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

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