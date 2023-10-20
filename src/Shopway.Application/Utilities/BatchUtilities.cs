using Shopway.Application.Features;
using Shopway.Domain.Abstractions;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Application.Utilities;

public static class BatchUtilities
{
    public static IList<BatchResponseEntry> NotErrorEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is not Error);
    }

    public static IList<BatchResponseEntry> InsertedEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey: struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Inserted);
    }

    public static bool IsInserted<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey: struct, IUniqueKey
    {
        return status is Inserted;
    }

    public static bool IsInserted<TResponseKey>(this BatchResponseEntry entry)
        where TResponseKey: struct, IUniqueKey
    {
        return entry.Status.IsInserted<TResponseKey>();
    }

    public static IList<BatchResponseEntry> UpdatedEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Updated);
    }

    public static bool IsUpdated<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey : struct, IUniqueKey
    {
        return status is Updated;
    }

    public static bool IsUpdated<TResponseKey>(this BatchResponseEntry entry)
        where TResponseKey : struct, IUniqueKey
    {
        return entry.Status.IsUpdated<TResponseKey>();
    }

    public static IList<BatchResponseEntry> ErrorEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
    where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Error);
    }

    public static bool IsError<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey : struct, IUniqueKey
    {
        return status is Error;
    }

    public static bool IsError<TResponseKey>(this BatchResponseEntry entry)
        where TResponseKey : struct, IUniqueKey
    {
        return entry.Status.IsError<TResponseKey>();
    }

    private static IList<BatchResponseEntry> Filter<TResponseKey>
    (
        BatchResponseBase<TResponseKey> response, 
        Func<BatchResponseEntry, bool> predicate
    )
        where TResponseKey: struct, IUniqueKey
    {
        return response
            .Entries
            .Where(predicate)
            .ToList();
    }
}