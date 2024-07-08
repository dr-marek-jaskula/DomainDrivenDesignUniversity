using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Features;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Application.Utilities;

public static class BatchUtilities
{
    public static bool AnyErrorEntry<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return response.Entries.Any(entry => entry.Status is Error);
    }

    public static IList<BatchResponseEntry<TResponseKey>> NotErrorEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is not Error);
    }

    public static IList<BatchResponseEntry<TResponseKey>> InsertedEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Inserted);
    }

    public static bool IsInserted<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey : struct, IUniqueKey
    {
        return status is Inserted;
    }

    public static bool IsInserted<TResponseKey>(this BatchResponseEntry<TResponseKey> entry)
        where TResponseKey : struct, IUniqueKey
    {
        return entry.Status.IsInserted<TResponseKey>();
    }

    public static IList<BatchResponseEntry<TResponseKey>> UpdatedEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
        where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Updated);
    }

    public static bool IsUpdated<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey : struct, IUniqueKey
    {
        return status is Updated;
    }

    public static bool IsUpdated<TResponseKey>(this BatchResponseEntry<TResponseKey> entry)
        where TResponseKey : struct, IUniqueKey
    {
        return entry.Status.IsUpdated<TResponseKey>();
    }

    public static IList<BatchResponseEntry<TResponseKey>> ErrorEntries<TResponseKey>(this BatchResponseBase<TResponseKey> response)
    where TResponseKey : struct, IUniqueKey
    {
        return Filter(response, entry => entry.Status is Error);
    }

    public static bool IsError<TResponseKey>(this BatchEntryStatus status)
        where TResponseKey : struct, IUniqueKey
    {
        return status is Error;
    }

    public static bool IsError<TResponseKey>(this BatchResponseEntry<TResponseKey> entry)
        where TResponseKey : struct, IUniqueKey
    {
        return entry.Status.IsError<TResponseKey>();
    }

    private static IList<BatchResponseEntry<TResponseKey>> Filter<TResponseKey>
    (
        BatchResponseBase<TResponseKey> response,
        Func<BatchResponseEntry<TResponseKey>, bool> predicate
    )
        where TResponseKey : struct, IUniqueKey
    {
        return response
            .Entries
            .Where(predicate)
            .ToList();
    }
}
