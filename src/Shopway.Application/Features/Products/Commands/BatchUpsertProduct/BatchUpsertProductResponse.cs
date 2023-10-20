using Shopway.Domain.EntityKeys;
using Shopway.Application.Abstractions.CQRS.Batch;

namespace Shopway.Application.Features.Products.Commands.BatchUpsertProduct;

public sealed record BatchUpsertProductResponse : BatchResponseBase<ProductKey>
{
    public BatchUpsertProductResponse(IList<BatchResponseEntry> entries)
        : base(entries)
    {
    }
}