using Shopway.Application.Abstractions.Batch;
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
        string ProductName,
        decimal Price,
        string UomCode,
        string Revision
    )
		: IBatchRequest;
}