using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityKeys;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineResponse : BatchResponseBase<OrderLineKey>
{
    public BatchUpsertOrderLineResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}