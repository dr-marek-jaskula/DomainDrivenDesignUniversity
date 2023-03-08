using Shopway.Domain.Entities;
using static Shopway.Application.Batch.BatchEntryStatus;
using static Shopway.Application.Mapping.ProductMapping;
using static Shopway.Application.Batch.Products.ProductBatchUpsertCommand;
using static Shopway.Application.Batch.Products.ProductBatchUpsertResponse;
using Shopway.Domain.EntitiesBusinessKeys;

namespace Shopway.Application.Batch.Products;

public static class ProductBatchUpsertCommandUtilities
{
    /// <summary>
    /// Product Key components list
    /// </summary>
    /// <returns>List of distinct product names</returns>
    public static IList<string> ProductNames(this ProductBatchUpsertCommand command)
    {
        return command
            .Requests
            .Select(x => x.ProductName)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Product Key components list
    /// </summary>
    /// <returns>List of distinct product revisions</returns>
    public static IList<string> ProductRevisions(this ProductBatchUpsertCommand command)
    {
        return command
            .Requests
            .Select(x => x.Revision)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Trim the batch command
    /// </summary>
    /// <param name="inputCommand">Not trimmed command</param>
    /// <returns>Trimmed command</returns>
    public static ProductBatchUpsertCommand Trim(this ProductBatchUpsertCommand inputCommand)
    {
        var requests = inputCommand
            .Requests
            .Select(Trim)
            .ToList();

        return new ProductBatchUpsertCommand(requests);
    }

    /// <summary>
    /// Trim the batch request
    /// </summary>
    /// <param name="inputRequest">Not trimmed request</param>
    /// <returns>Trimmed request</returns>
    private static ProductBatchUpsertRequest Trim(this ProductBatchUpsertRequest inputRequest)
    {
        return new ProductBatchUpsertRequest
        (
            inputRequest.ProductName.Trim(),
            inputRequest.Price,
            inputRequest.UomCode.Trim(),
            inputRequest.Revision.Trim()
        );
    }

    /// <summary>
    /// Get update requests, so requests that are present in productsToUpdateWithKeys dictionary
    /// </summary>
    /// <param name="command"></param>
    /// <param name="productsToUpdateWithKeys"></param>
    /// <returns></returns>
    public static IReadOnlyList<ProductBatchUpsertRequest> GetUpdateRequests
    (
        this ProductBatchUpsertCommand command,
        IDictionary<ProductKey, Product> productsToUpdateWithKeys
    )
    {
        return command
            .FilterValidRequests(productsToUpdateWithKeys, Updated);
    }

    public static IReadOnlyList<ProductBatchUpsertRequest> GetInsertRequests
    (
        this ProductBatchUpsertCommand command,
        IDictionary<ProductKey, Product> productsToUpdateWithKeys
    )
    {
        return command
            .FilterValidRequests(productsToUpdateWithKeys, Inserted);
    }

    /// <summary>
    /// Filter command to obtain valid requests based on a input status
    /// </summary>
    /// <param name="command">Batch command</param>
    /// <param name="productsToUpdateWithKeys">Products to be updated with keys</param>
    /// <param name="status">Insert or Updated status</param>
    /// <returns>Filtered valid requests</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IReadOnlyList<ProductBatchUpsertRequest> FilterValidRequests
    (
        this ProductBatchUpsertCommand command,
        IDictionary<ProductKey, Product> productsToUpdateWithKeys,
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
            .Where(request => productsToUpdateWithKeys.ContainsKey(MapFromRequestToResponseKey(request)) == searchCondition)
            .ToList()
            .AsReadOnly();
    }
}