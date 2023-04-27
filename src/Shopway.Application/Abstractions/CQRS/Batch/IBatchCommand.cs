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
public interface IBatchCommand<TBatchRequest, out TBatchResponse> : ICommand<TBatchResponse>
    where TBatchRequest : IBatchRequest
    where TBatchResponse : IBatchResponse
{
    IList<TBatchRequest> Requests { get; }
}