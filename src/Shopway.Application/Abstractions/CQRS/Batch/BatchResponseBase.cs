using Shopway.Application.Features;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public abstract record BatchResponseBase<TResponseKey> : IBatchResponse<TResponseKey>
    where TResponseKey : struct, IUniqueKey
{
    protected BatchResponseBase(IList<BatchResponseEntry<TResponseKey>> entries)
    {
        Entries = entries;
    }

    public IList<BatchResponseEntry<TResponseKey>> Entries { get; set; }
}
