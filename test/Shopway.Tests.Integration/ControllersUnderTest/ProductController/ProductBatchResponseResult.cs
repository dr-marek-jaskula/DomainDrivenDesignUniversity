using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Batch;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public sealed record ProductBatchResponseResult
{
    public ProductBatchResponseResult(IList<ProductBatchResponseTestEntry> entries)
    {
        Entries = entries;
    }

    public IList<ProductBatchResponseTestEntry> Entries { get; set; }

    public sealed record ProductBatchResponseTestEntry
    (
        ProductTestKey Key,
        BatchEntryStatus Status,
        IList<string> Errors
    );

    public sealed record ProductTestKey(string ProductName, string Revision);
}