using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityKeys;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

public sealed record BatchUpsertProductCommand : IBatchCommand<ProductBatchUpsertRequest, BatchUpsertProductResponse>
{
    public BatchUpsertProductCommand(IList<ProductBatchUpsertRequest> requests)
    {
        Requests = requests;
    }

    public IList<ProductBatchUpsertRequest> Requests { get; set; }

    public sealed record ProductBatchUpsertRequest
    (
        ProductKey ProductKey,
        decimal Price,
        string UomCode
    )
        : IBatchRequest;
}