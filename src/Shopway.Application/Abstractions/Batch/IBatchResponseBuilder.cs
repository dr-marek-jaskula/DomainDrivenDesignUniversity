using Shopway.Application.Batch;

namespace Shopway.Application.Abstractions.Batch;

/// <summary>
/// Builder used to validate all requests in the batch command and then create the list of response entries
/// Uses IBatchResponseEntryBuilders to validate respective requests
/// </summary>
/// <typeparam name="TBatchRequest">Batch Request type</typeparam>
/// <typeparam name="TBatchResponseKey">Unique Response Key type</typeparam>
public interface IBatchResponseBuilder<TBatchRequest, TBatchResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TBatchResponseKey : struct, IBatchResponseKey
{
    /// <summary>
    /// Batch requests that has not Error status
    /// </summary>
    IReadOnlyList<TBatchRequest> ValidRequests { get; }

    /// <summary>
    /// Batch requests that has Insert status
    /// </summary>
    IReadOnlyList<TBatchRequest> ValidRequestsToInsert { get; }

    /// <summary>
    /// Batch requests that has Update status
    /// </summary>
    IReadOnlyList<TBatchRequest> ValidRequestsToUpdate { get; }

    /// <summary>
    /// Required to use the builder. Set the request to response key to mapping method
    /// </summary>
    /// <param name="mapFromRequestToResponseKey">Request to ResponseKey mapping method</param>
    void SetRequestToResponseKeyMapper(Func<TBatchRequest, TBatchResponseKey> mapFromRequestToResponseKey);

    /// <summary>
    /// Build response entries: key, status and errors if there are some
    /// </summary>
    /// <returns></returns>
    IList<BatchResponseEntry> BuildResponseEntries();

    /// <summary>
    /// Validate requests that are supposed to insert an entity
    /// </summary>
    /// <param name="requests">Insert requests</param>
    /// <param name="requestValidationMethod">Delegate that validates the request</param>
    /// <returns></returns>
    IBatchResponseBuilder<TBatchRequest, TBatchResponseKey> ValidateInsertRequests
    (
        IReadOnlyList<TBatchRequest> requests, 
        Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
    );

    /// <summary>
    /// Validate requests that are supposed to update an entity
    /// </summary>
    /// <param name="requests">Update requests</param>
    /// <param name="requestValidationMethod">Delegate that validates the request</param>
    /// <returns></returns>
    IBatchResponseBuilder<TBatchRequest, TBatchResponseKey> ValidateUpdateRequests
    (
        IReadOnlyList<TBatchRequest> requests, 
        Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
    );
}