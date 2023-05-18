using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityKeys;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

public sealed record BatchUpsertProductResponse : BatchResponseBase<ProductKey>
{
    public BatchUpsertProductResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}