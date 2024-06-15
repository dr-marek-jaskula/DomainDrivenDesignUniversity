using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineResponse : BatchResponseBase<OrderLineKey>
{
    public BatchUpsertOrderLineResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}
