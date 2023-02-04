using Shopway.Application.Batch;

namespace Shopway.Application.Abstractions.Batch;

public interface IBatchResponse : IResponse
{
    IList<BatchResponseEntry> Entries { get; set; }
}