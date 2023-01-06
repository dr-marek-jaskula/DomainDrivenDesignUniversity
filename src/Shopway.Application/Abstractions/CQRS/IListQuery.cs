using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IListQuery<TResponse> : IRequest<IResult<IList<TResponse>>>
    where TResponse : IResponse
{
}