using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.Batch;

/// <summary>
/// Represents the batch command handler interface
/// </summary>
/// <typeparam name="TBatchCommand">The batch command type</typeparam>
public interface IBatchCommandHandler<in TBatchCommand, TBatchRequest> : IRequestHandler<TBatchCommand, IResult>
    where TBatchCommand : IBatchCommand<TBatchRequest>
    where TBatchRequest : IBatchRequest
{
}

/// <summary>
/// Represents the batch command handler interface
/// </summary>
/// <typeparam name="TBatchCommand">The batch command type</typeparam>
/// <typeparam name="TBatchResponse">The batch command response type</typeparam>
public interface IBatchCommandHandler<in TBatchCommand, TBatchRequest, TBatchResponse> : IRequestHandler<TBatchCommand, IResult<TBatchResponse>>
    where TBatchCommand : IBatchCommand<TBatchRequest, TBatchResponse>
    where TBatchResponse : IBatchResponse
    where TBatchRequest : IBatchRequest
{
}
