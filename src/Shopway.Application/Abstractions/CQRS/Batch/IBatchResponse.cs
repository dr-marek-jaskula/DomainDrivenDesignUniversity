using Shopway.Application.CQRS;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public interface IBatchResponse : IResponse
{
    IList<BatchResponseEntry> Entries { get; set; }
}