using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.EntityBusinessKeys;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.ProductBatchUpsertCommand;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

public sealed record ProductBatchUpsertCommand : IBatchCommand<ProductBatchUpsertRequest, ProductBatchUpsertResponse>
{
    public ProductBatchUpsertCommand(IList<ProductBatchUpsertRequest> requests)
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