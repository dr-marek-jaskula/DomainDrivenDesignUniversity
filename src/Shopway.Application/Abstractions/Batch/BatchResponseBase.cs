using Shopway.Application.Batch;

namespace Shopway.Application.Abstractions.Batch;

public abstract record BatchResponseBase<TBatchResponseKey> : IBatchResponse
    where TBatchResponseKey : class, IBatchResponseKey
{
	protected BatchResponseBase(IList<BatchResponseEntry> entries)
	{
		Entries = entries;
	}

	public IList<BatchResponseEntry> Entries { get; set; }
}