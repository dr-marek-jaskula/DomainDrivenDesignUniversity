using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

public delegate IBatchResponseBuilder<TBatchRequest, TResponseKey> CreateBatchResponseBuilder<TBatchRequest, TResponseKey>(Func<TBatchRequest, TResponseKey> mapFromRequestToResponseKey)
    where TBatchRequest : class, IBatchRequest
    where TResponseKey : struct, IUniqueKey;
