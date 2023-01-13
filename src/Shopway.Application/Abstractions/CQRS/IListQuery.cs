using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the list query interface
/// </summary>
/// <typeparam name="TResponse">The list query response type</typeparam>
public interface IListQuery<TResponse> : IRequest<IResult<IList<TResponse>>>
    where TResponse : IResponse
{
}