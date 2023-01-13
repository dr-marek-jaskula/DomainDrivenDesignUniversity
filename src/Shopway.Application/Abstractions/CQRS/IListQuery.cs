using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

public interface IListQuery<TResponse> : IRequest<IResult<IList<TResponse>>>
    where TResponse : IResponse
{
}