using Shopway.Application.Abstractions.Batch;
using Shopway.Domain.EntitiesBusinessKeys;

namespace Shopway.Application.Batch.Products;

public sealed record ProductBatchUpsertResponse : BatchResponseBase<ProductKey>
{
	public ProductBatchUpsertResponse(IList<BatchResponseEntry> entries)
		: base(entries)
	{
	}
}