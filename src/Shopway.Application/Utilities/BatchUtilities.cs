using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Batch;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Application.Utilities;

public static class BatchUtilities
{
    public static IList<BatchResponseEntry> NotErrorEntries<TBatchResponseKey>(this BatchResponseBase<TBatchResponseKey> response)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return Filter(response, entry => entry.Status is not Error);
    }

    public static IList<BatchResponseEntry> InsertedEntries<TBatchResponseKey>(this BatchResponseBase<TBatchResponseKey> response)
        where TBatchResponseKey: struct, IBatchResponseKey
    {
        return Filter(response, entry => entry.Status is Inserted);
    }

    public static bool IsInserted<TBatchResponseKey>(this BatchEntryStatus status)
        where TBatchResponseKey: struct, IBatchResponseKey
    {
        return status is Inserted;
    }

    public static bool IsInserted<TBatchResponseKey>(this BatchResponseEntry entry)
        where TBatchResponseKey: struct, IBatchResponseKey
    {
        return entry.Status.IsInserted<TBatchResponseKey>();
    }

    public static IList<BatchResponseEntry> UpdatedEntries<TBatchResponseKey>(this BatchResponseBase<TBatchResponseKey> response)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return Filter(response, entry => entry.Status is Updated);
    }

    public static bool IsUpdated<TBatchResponseKey>(this BatchEntryStatus status)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return status is Updated;
    }

    public static bool IsUpdated<TBatchResponseKey>(this BatchResponseEntry entry)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return entry.Status.IsUpdated<TBatchResponseKey>();
    }

    public static IList<BatchResponseEntry> ErrorEntries<TBatchResponseKey>(this BatchResponseBase<TBatchResponseKey> response)
    where TBatchResponseKey : struct, IBatchResponseKey
    {
        return Filter(response, entry => entry.Status is Error);
    }

    public static bool IsError<TBatchResponseKey>(this BatchEntryStatus status)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return status is Error;
    }

    public static bool IsError<TBatchResponseKey>(this BatchResponseEntry entry)
        where TBatchResponseKey : struct, IBatchResponseKey
    {
        return entry.Status.IsError<TBatchResponseKey>();
    }

    private static IList<BatchResponseEntry> Filter<TBatchResponseKey>
    (
        BatchResponseBase<TBatchResponseKey> response, 
        Func<BatchResponseEntry, bool> predicate
    )
        where TBatchResponseKey: struct, IBatchResponseKey
    {
        return response
            .Entries
            .Where(predicate)
            .ToList();
    }
}