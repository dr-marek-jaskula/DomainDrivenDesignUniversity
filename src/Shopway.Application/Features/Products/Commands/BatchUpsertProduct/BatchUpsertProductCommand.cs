using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityKeys;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Features.Products.Commands.BatchUpsertProduct;

public sealed record BatchUpsertProductCommand : IBatchCommand<ProductBatchUpsertRequest, BatchUpsertProductResponse, ProductKey>
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
