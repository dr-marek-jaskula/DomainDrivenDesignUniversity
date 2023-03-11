using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.CQRS;
using Shopway.Domain.EntityBusinessKeys;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

public sealed record ProductBatchUpsertResponse : BatchResponseBase<ProductKey>
{
    public ProductBatchUpsertResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}