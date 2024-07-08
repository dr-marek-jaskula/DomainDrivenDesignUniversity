using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Application.Abstractions.CQRS.Batch;

/// <summary>
/// Represents the batch command interface
/// </summary>
public interface IBatchCommand<TBatchRequest> : ICommand
    where TBatchRequest : IBatchRequest
{
    IList<TBatchRequest> Requests { get; }
}

/// <summary>
/// Represents the batch command interface
/// </summary>
/// <typeparam name="TBatchResponse">The command response type</typeparam>
public interface IBatchCommand<TBatchRequest, out TBatchResponse, TResponseKey> : ICommand<TBatchResponse>
    where TBatchRequest : IBatchRequest
    where TBatchResponse : IBatchResponse<TResponseKey>
    where TResponseKey : IUniqueKey
{
    IList<TBatchRequest> Requests { get; }
}
