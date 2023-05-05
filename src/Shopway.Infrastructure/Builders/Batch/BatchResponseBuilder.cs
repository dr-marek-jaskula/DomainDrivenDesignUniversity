using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.CQRS.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

public sealed partial class BatchResponseBuilder<TBatchRequest, TResponseKey> : IBatchResponseBuilder<TBatchRequest, TResponseKey> 
    where TBatchRequest : class, IBatchRequest
    where TResponseKey : struct, IBusinessKey
{
    /// <summary>
    /// Required delegate. Used to map the requests to response keys. 
    /// Due to the fact that the builder will be injected from the Dependency Injection Container, it needs to be set after the injection.
    /// It is required to provide this delegate. 
    /// </summary>
    private Func<TBatchRequest, TResponseKey>? _mapFromRequestToResponseKey;

    /// <summary>
    /// (ResponseKey, ResponseEntryBuilder) dictionary to store builder for all requests and allow to deal with duplicates in the easy way
    /// </summary>
    private readonly IDictionary<TResponseKey, BatchResponseEntryBuilder> _responseEntryBuilders;

    public BatchResponseBuilder()
    {
        _responseEntryBuilders = new Dictionary<TResponseKey, BatchResponseEntryBuilder>();
    }

    public IReadOnlyList<TBatchRequest> ValidRequests => Filter(builder => builder.IsValid).AsReadOnly();
    public IReadOnlyList<TBatchRequest> ValidRequestsToInsert => Filter(builder => builder.IsValidAndToInsert).AsReadOnly();
    public IReadOnlyList<TBatchRequest> ValidRequestsToUpdate => Filter(builder => builder.IsValidAndToUpdate).AsReadOnly();

    /// <summary>
    /// Filter builders on given predicate and retrieve the requests from filtered builders
    /// </summary>
    /// <param name="predicate">Predicate used to filter builders</param>
    /// <returns>A list of requests from filtered builders</returns>
    private IList<TBatchRequest> Filter(Func<BatchResponseEntryBuilder, bool> predicate)
    {
        return _responseEntryBuilders
            .Values
            .OfType<BatchResponseEntryBuilder>()
            .Where(predicate)
            .Select(builder => builder.Request)
            .ToList();
    }

    /// <summary>
    /// Provides the way how to get the response key from the request. 
    /// This mapper must be provided before validation.
    /// </summary>
    /// <param name="mapFromRequestToResponseKey"></param>
    public void SetRequestToResponseKeyMapper
    (
        Func<TBatchRequest, TResponseKey> mapFromRequestToResponseKey
    )
    {
        _mapFromRequestToResponseKey = mapFromRequestToResponseKey;
    }

    /// <summary>
    /// The builder output.
    /// </summary>
    /// <returns>List of all responseEntries</returns>
    public IList<BatchResponseEntry> BuildResponseEntries()
    {
        return _responseEntryBuilders
            .Values
            .Select(builder => builder.BuildBatchResponseEntry())
            .ToList();
    }

    /// <summary>
    /// Validates the requests and for each of them sets success status to 'Inserted'. Status will be changed to 'Error' if at least one error occurs
    /// </summary>
    /// <param name="insertRequests">Requests that are meant to be used to insert entities</param>
    /// <param name="requestValidationMethod">Validation method that will be performed over the each provided request</param>
    /// <returns>Builder to be able to chain subsequent validation method</returns>
    public IBatchResponseBuilder<TBatchRequest, TResponseKey> ValidateInsertRequests
    (
        IReadOnlyList<TBatchRequest> insertRequests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod
    )
    {
        return Validate(insertRequests, requestValidationMethod, Inserted);
    }

    /// <summary>
    /// Validates the requests and for each of them sets success status to 'Updated'. Status will be change to 'Error' if at least one error occurs
    /// </summary>
    /// <param name="updateRequests">Requests that are meant to be used to update entities</param>
    /// <param name="requestValidationMethod">Validation method that will be performed over the each provided request</param>
    /// <returns>Builder to be able to chain subsequent validation method</returns>
    public IBatchResponseBuilder<TBatchRequest, TResponseKey> ValidateUpdateRequests
    (
        IReadOnlyList<TBatchRequest> updateRequests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod
    )
    {
        return Validate(updateRequests, requestValidationMethod, Updated);
    }

    /// <summary>
    /// Method that creates builders for each input request (if necessary) and then perform the validation, using the passed request validation method
    /// </summary>
    /// <param name="requests">Requests to be validated</param>
    /// <param name="requestValidationMethod">The validation manner</param>
    /// <param name="successStatus">Status that will be used if the validation succeeds</param>
    /// <returns></returns>
    private BatchResponseBuilder<TBatchRequest, TResponseKey> Validate
    (
        IReadOnlyList<TBatchRequest> requests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod,
        BatchEntryStatus successStatus
    )
    {
        if (requests is null)
        {
            return this;
        }

        foreach (var request in requests)
        {
            var responseEntryBuilder = CreateResponseEntryBuilder(request, successStatus);
            requestValidationMethod(responseEntryBuilder, request);
        }

        return this;
    }

    /// <summary>
    /// Creates the builder for a given request if there are no such builder. 
    /// To created the builder, first the ReponseKey needs to be created, so the key mapper must be provided.
    /// If there is one, then set this builder status to error and add duplicated request error.
    /// </summary>
    /// <param name="request">Request for which the builder will be created</param>
    /// <param name="successStatus">Status that will used a builder success status</param>
    /// <returns>Created request entry builder</returns>
    /// <exception cref="InvalidOperationException">The Request-to-ResponseKey mapper must be provided before the use of this method</exception>
    private IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> CreateResponseEntryBuilder
    (
        TBatchRequest request,
        BatchEntryStatus successStatus
    )
    {
        if (_mapFromRequestToResponseKey is null)
        {
            throw new InvalidOperationException($"The Request-to-ResponseKey mapper is null. Use '{nameof(SetRequestToResponseKeyMapper)}' method to set mapper.");
        }

        var responseKey = _mapFromRequestToResponseKey(request);

        if (_responseEntryBuilders.TryGetValue(responseKey, out var responseEntryBuilder))
        {
            bool isDuplicated = true;
            responseEntryBuilder.If(isDuplicated, thenError: DuplicatedRequest(responseKey));
            return responseEntryBuilder;
        }

        responseEntryBuilder = new BatchResponseEntryBuilder(request, responseKey, successStatus);
        _responseEntryBuilders.Add(responseKey, responseEntryBuilder);

        return responseEntryBuilder;
    }
}