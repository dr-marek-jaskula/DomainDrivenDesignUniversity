using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Batch;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

public sealed partial class BatchResponseBuilder<TBatchRequest, TBatchResponseKey> : IBatchResponseBuilder<TBatchRequest, TBatchResponseKey> where TBatchRequest : class, IBatchRequest
    where TBatchResponseKey : class, IBatchResponseKey
{
    //Delegate used to map request to response key
    private Func<TBatchRequest, TBatchResponseKey>? _mapFromRequestToResponseKey;

    //ResponseEntry is crated using the request and the response key
    private readonly IDictionary<TBatchResponseKey, BatchResponseEntryBuilder> _responseEntryBuilders;

    public BatchResponseBuilder()
    {
        _responseEntryBuilders = new Dictionary<TBatchResponseKey, BatchResponseEntryBuilder>();
    }

    public IReadOnlyList<TBatchRequest> ValidRequests => Filter(builder => builder.IsValid()).AsReadOnly();
    public IReadOnlyList<TBatchRequest> ValidRequestsToInsert => Filter(builder => builder.IsValidAndToInsert()).AsReadOnly();
    public IReadOnlyList<TBatchRequest> ValidRequestsToUpdate => Filter(builder => builder.IsValidAndToUpdate()).AsReadOnly();

    public void SetRequestToResponseKeyMapper
    (
        Func<TBatchRequest, TBatchResponseKey> mapFromRequestToResponseKey
    )
    {
        _mapFromRequestToResponseKey = mapFromRequestToResponseKey;
    }

    public IList<BatchResponseEntry> BuildResponseEntries()
    {
        return _responseEntryBuilders
            .Values
            .Select(request => request.ToBatchResponseEntry())
            .ToList();
    }

    public IBatchResponseBuilder<TBatchRequest, TBatchResponseKey> ValidateInsertRequests
    (
        IReadOnlyList<TBatchRequest> requests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
    )
    {
        return Validate(requests, requestValidationMethod, Inserted);
    }

    public IBatchResponseBuilder<TBatchRequest, TBatchResponseKey> ValidateUpdateRequests
    (
        IReadOnlyList<TBatchRequest> requests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
    )
    {
        return Validate(requests, requestValidationMethod, Updated);
    }

    private IList<TBatchRequest> Filter(Func<BatchResponseEntryBuilder, bool> predicate)
    {
        return _responseEntryBuilders
            .Values
            .OfType<BatchResponseEntryBuilder>()
            .Where(predicate)
            .Select(builder => builder.Request)
            .ToList();
    }

    private BatchResponseBuilder<TBatchRequest, TBatchResponseKey> Validate
    (
        IReadOnlyList<TBatchRequest> requests,
        Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod,
        BatchEntryStatus successStatus
    )
    {
        if (requests is null)
        {
            return this;
        }

        foreach (var request in requests)
        {
            var builder = CreateRequestBuilder(request, successStatus);
            requestValidationMethod(builder, request);
        }

        return this;
    }

    private IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> CreateRequestBuilder
    (
        TBatchRequest request,
        BatchEntryStatus successStatus
    )
    {
        if (_mapFromRequestToResponseKey is null)
        {
            throw new ArgumentNullException("The Request to ResponseKey mapper is not set");
        }

        var key = _mapFromRequestToResponseKey(request);

        if (_responseEntryBuilders.TryGetValue(key, out var builder))
        {
            bool isInvalid = true;
            builder.If(isInvalid, $"Duplicated request for key {key}");
            return builder;
        }

        builder = new BatchResponseEntryBuilder(request, key, successStatus);
        _responseEntryBuilders.Add(key, builder);

        return builder;
    }
}