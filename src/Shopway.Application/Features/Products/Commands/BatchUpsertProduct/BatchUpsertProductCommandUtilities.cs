using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using static Shopway.Application.Features.BatchEntryStatus;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;
using static Shopway.Application.Mappings.ProductMapping;

namespace Shopway.Application.Features.Products.Commands.BatchUpsertProduct;

internal static class BatchUpsertProductCommandUtilities
{
    /// <summary>
    /// Product Key components list
    /// </summary>
    /// <returns>List of distinct product names</returns>
    public static List<string> ProductNames(this BatchUpsertProductCommand command)
    {
        return command
            .Requests
            .Select(x => x.ProductKey.ProductName)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Product Key components list
    /// </summary>
    /// <returns>List of distinct product revisions</returns>
    public static List<string> ProductRevisions(this BatchUpsertProductCommand command)
    {
        return command
            .Requests
            .Select(x => x.ProductKey.Revision)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Trim the batch command
    /// </summary>
    /// <param name="inputCommand">Not trimmed command</param>
    /// <returns>Trimmed command</returns>
    public static BatchUpsertProductCommand Trim(this BatchUpsertProductCommand inputCommand)
    {
        var requests = inputCommand
            .Requests
            .Select(Trim)
            .ToList();

        return new BatchUpsertProductCommand(requests);
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
            inputRequest.ProductKey,
            inputRequest.Price,
            inputRequest.UomCode.Trim()
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
        this BatchUpsertProductCommand command,
        IDictionary<ProductKey, Product> productsToUpdateWithKeys
    )
    {
        return command
            .FilterValidRequests(productsToUpdateWithKeys, Updated);
    }

    public static IReadOnlyList<ProductBatchUpsertRequest> GetInsertRequests
    (
        this BatchUpsertProductCommand command,
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
        this BatchUpsertProductCommand command,
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
            .Where(request => productsToUpdateWithKeys.ContainsKey(request.ToProductKey()) == searchCondition)
            .ToList()
            .AsReadOnly();
    }
}
