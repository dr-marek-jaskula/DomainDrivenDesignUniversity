using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.Batch;

/// <summary>
/// Represents the batch command interface
/// </summary>
public interface IBatchCommand<TBatchRequest> : IRequest<IResult>
    where TBatchRequest : IBatchRequest
{
    IList<TBatchRequest> Requests { get; }
}

/// <summary>
/// Represents the batch command interface
/// </summary>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface IBatchCommand<TBatchRequest, out TResponse> : IRequest<IResult<TResponse>>
    where TBatchRequest : IBatchRequest
    where TResponse : IBatchResponse
{
    IList<TBatchRequest> Requests { get; }
}