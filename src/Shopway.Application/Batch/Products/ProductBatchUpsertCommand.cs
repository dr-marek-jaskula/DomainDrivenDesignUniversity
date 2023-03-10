using Shopway.Application.Abstractions.Batch;
using Shopway.Domain.EntityBusinessKeys;
using static Shopway.Application.Batch.Products.ProductBatchUpsertCommand;

namespace Shopway.Application.Batch.Products;

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