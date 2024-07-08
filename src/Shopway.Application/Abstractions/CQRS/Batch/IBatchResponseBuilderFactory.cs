using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public interface IBatchResponseBuilderFactory
{
    IBatchResponseBuilder<TBatchRequest, TResponseKey> Create<TBatchRequest, TResponseKey>(Func<TBatchRequest, TResponseKey> mapFromRequestToResponseKey)
        where TBatchRequest : class, IBatchRequest
        where TResponseKey : struct, IUniqueKey;
}
