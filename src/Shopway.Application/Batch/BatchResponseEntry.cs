using Shopway.Application.Abstractions.Batch;

namespace Shopway.Application.Batch;

public sealed record BatchResponseEntry
(
    IBatchResponseKey Key,
    BatchEntryStatus Status,
    IList<string> Errors
);