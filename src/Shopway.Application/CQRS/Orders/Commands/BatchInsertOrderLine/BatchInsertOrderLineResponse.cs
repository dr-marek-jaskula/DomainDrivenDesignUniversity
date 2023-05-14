using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityBusinessKeys;

namespace Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;

public sealed record BatchInsertOrderLineResponse : BatchResponseBase<OrderLineKey>
{
    public BatchInsertOrderLineResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}