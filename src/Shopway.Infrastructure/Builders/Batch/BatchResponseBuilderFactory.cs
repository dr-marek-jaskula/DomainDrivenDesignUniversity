using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Infrastructure.Builders.Batch;

public sealed class BatchResponseBuilderFactory : IBatchResponseBuilderFactory
{
    public IBatchResponseBuilder<TBatchRequest, TResponseKey> Create<TBatchRequest, TResponseKey>(Func<TBatchRequest, TResponseKey> mapFromRequestToResponseKey)
        where TBatchRequest : class, IBatchRequest
        where TResponseKey : struct, IUniqueKey
    {
        return new BatchResponseBuilder<TBatchRequest, TResponseKey>(mapFromRequestToResponseKey);
    }

    public static IBatchResponseBuilder<TBatchRequest, TResponseKey> CreateBuilder<TBatchRequest, TResponseKey>(Func<TBatchRequest, TResponseKey> mapFromRequestToResponseKey)
        where TBatchRequest : class, IBatchRequest
        where TResponseKey : struct, IUniqueKey
    {
        return new BatchResponseBuilder<TBatchRequest, TResponseKey>(mapFromRequestToResponseKey);
    }
}
