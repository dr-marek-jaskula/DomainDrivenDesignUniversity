using Shopway.Application.Features;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public interface IBatchResponse<TResponseKey> : IResponse
    where TResponseKey : IUniqueKey
{
    IList<BatchResponseEntry<TResponseKey>> Entries { get; set; }
}
