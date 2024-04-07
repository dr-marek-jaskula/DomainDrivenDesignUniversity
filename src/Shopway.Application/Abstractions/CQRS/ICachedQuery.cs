namespace Shopway.Application.Abstractions.CQRS;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Duration { get; }
}

public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery
    where TResponse : IResponse;