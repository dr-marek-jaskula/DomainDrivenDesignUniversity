using Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;
using Shopway.Application.Mappings;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using static Shopway.Application.CQRS.BatchEntryStatus;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

internal static class BatchUpsertOrderLineCommandUtilities
{
    /// <summary>
    /// Get update requests, so requests that are present in orderLinesToUpdateWithKeys dictionary
    /// </summary>
    /// <param name="command"></param>
    /// <param name="orderLinesToUpdateWithKeys"></param>
    /// <returns></returns>
    public static IReadOnlyList<BatchUpsertOrderLineRequest> GetUpdateRequests
    (
        this BatchUpsertOrderLineCommand command,
        IDictionary<OrderLineKey, OrderLine> orderLinesToUpdateWithKeys
    )
    {
        return command
            .FilterValidRequests(orderLinesToUpdateWithKeys, Updated);
    }

    public static IReadOnlyList<BatchUpsertOrderLineRequest> GetInsertRequests
    (
        this BatchUpsertOrderLineCommand command,
        IDictionary<OrderLineKey, OrderLine> orderLinesToUpdateWithKeys
    )
    {
        return command
            .FilterValidRequests(orderLinesToUpdateWithKeys, Inserted);
    }

    /// <summary>
    /// Filter command to obtain valid requests based on a input status
    /// </summary>
    /// <param name="command">Batch command</param>
    /// <param name="orderLinesToUpdateWithKeys">Products to be updated with keys</param>
    /// <param name="status">Insert or Updated status</param>
    /// <returns>Filtered valid requests</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IReadOnlyList<BatchUpsertOrderLineRequest> FilterValidRequests
    (
        this BatchUpsertOrderLineCommand command,
        IDictionary<OrderLineKey, OrderLine> orderLinesToUpdateWithKeys,
        BatchEntryStatus status
    )
    {
        if (status is Error)
        {
            throw new ArgumentException($"Status must differ from {Error}");
        }

        //If status is updated, use productsToUpdate. Otherwise, exclude them, so only insert requests will remain.
        bool searchCondition = status is Updated;

        return command
            .Requests
            .Where(request => orderLinesToUpdateWithKeys.ContainsKey(request.ToOrderLineKey()) == searchCondition)
            .ToList()
            .AsReadOnly();
    }
}