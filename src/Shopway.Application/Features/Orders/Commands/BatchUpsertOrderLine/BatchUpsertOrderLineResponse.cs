using Shopway.Domain.EntityKeys;
using Shopway.Application.Abstractions.CQRS.Batch;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

public sealed record BatchUpsertOrderLineResponse : BatchResponseBase<OrderLineKey>
{
    public BatchUpsertOrderLineResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}