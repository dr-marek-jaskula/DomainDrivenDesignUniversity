namespace Shopway.Application.Abstractions.CQRS;

public interface IPageQuery<out TResponse> : IQuery<TResponse>
    where TResponse : IResponse
{

}