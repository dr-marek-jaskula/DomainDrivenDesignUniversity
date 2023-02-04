﻿using Shopway.Application.Abstractions.Batch;
using static Shopway.Application.Batch.Products.ProductBatchUpsertResponse;

namespace Shopway.Application.Batch.Products;

public sealed record ProductBatchUpsertResponse : BatchResponseBase<ProductKey>
{
	public ProductBatchUpsertResponse(IList<BatchResponseEntry> entries)
		: base(entries)
	{
	}

	public sealed record ProductKey(string ProductName, string Revision) : IBatchResponseKey;
}