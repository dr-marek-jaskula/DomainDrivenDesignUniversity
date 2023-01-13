using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the list query handler interface
/// </summary>
/// <typeparam name="TQuery">The list query type</typeparam>
/// <typeparam name="TResponse">The list query response type</typeparam>
public interface IListQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, IResult<IList<TResponse>>>
    where TQuery : IListQuery<TResponse>
    where TResponse : IResponse
{
}