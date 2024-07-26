using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineCommand(IList<BatchUpsertOrderLineRequest> Requests, OrderHeaderId OrderHeaderId)
    : IBatchCommand<BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse, OrderLineKey>
{
    public IList<BatchUpsertOrderLineRequest> Requests { get; init; } = Requests;
    public OrderHeaderId OrderHeaderId { get; init; } = OrderHeaderId;

    public sealed record BatchUpsertOrderLineRequest
    (
        ProductId ProductId,
        int Amount,
        decimal? Discount
    )
        : IBatchRequest;
}
