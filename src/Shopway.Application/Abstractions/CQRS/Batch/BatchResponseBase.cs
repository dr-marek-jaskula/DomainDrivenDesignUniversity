using Shopway.Application.Features;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public abstract record BatchResponseBase<TResponseKey> : IBatchResponse
    where TResponseKey : struct, IUniqueKey
{
    protected BatchResponseBase(IList<BatchResponseEntry> entries)
    {
        Entries = entries;
    }

    public IList<BatchResponseEntry> Entries { get; set; }
}