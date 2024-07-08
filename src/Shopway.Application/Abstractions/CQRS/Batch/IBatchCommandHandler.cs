using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

/// <summary>
/// Represents the batch command handler interface
/// </summary>
/// <typeparam name="TBatchCommand">The batch command type</typeparam>
public interface IBatchCommandHandler<in TBatchCommand, TBatchRequest> : ICommandHandler<TBatchCommand>
    where TBatchCommand : IBatchCommand<TBatchRequest>
    where TBatchRequest : IBatchRequest
{
}

/// <summary>
/// Represents the batch command handler interface
/// </summary>
/// <typeparam name="TBatchCommand">The batch command type</typeparam>
/// <typeparam name="TBatchResponse">The batch command response type</typeparam>
public interface IBatchCommandHandler<in TBatchCommand, TBatchRequest, TBatchResponse, TResponseKey> : ICommandHandler<TBatchCommand, TBatchResponse>
    where TBatchCommand : IBatchCommand<TBatchRequest, TBatchResponse, TResponseKey>
    where TBatchResponse : IBatchResponse<TResponseKey>
    where TBatchRequest : IBatchRequest
    where TResponseKey : IUniqueKey
{
}
