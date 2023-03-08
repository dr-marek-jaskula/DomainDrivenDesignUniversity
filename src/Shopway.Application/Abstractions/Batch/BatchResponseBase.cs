using Shopway.Application.Batch;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.Batch;

public abstract record BatchResponseBase<TResponseKey> : IBatchResponse
    where TResponseKey : struct, IBusinessKey
{
	protected BatchResponseBase(IList<BatchResponseEntry> entries)
	{
		Entries = entries;
	}

	public IList<BatchResponseEntry> Entries { get; set; }
}