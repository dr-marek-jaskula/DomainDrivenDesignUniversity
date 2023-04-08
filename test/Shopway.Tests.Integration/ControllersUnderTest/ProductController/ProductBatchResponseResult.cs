using Shopway.Application.CQRS;
using Shopway.Domain.Errors;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

/// <summary>
/// The helper class, created to be able to deserialize the batch response. 
/// The ProductTestKey is required for this purpose (but the source ProductKey can be also used)
/// </summary>
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
        IList<Error> Errors
    );

    public sealed record ProductTestKey(string ProductName, string Revision);
}