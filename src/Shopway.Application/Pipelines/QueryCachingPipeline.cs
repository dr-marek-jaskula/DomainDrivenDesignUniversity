using MediatR;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Application.Pipelines;

public sealed class QueryCachingPipeline<TQuery, TResultOfResponse>(IFusionCache fusionCache) : IPipelineBehavior<TQuery, TResultOfResponse>
    where TQuery : class, IRequest<TResultOfResponse>, IQuery<IResponse>, ICachedQuery
    where TResultOfResponse : IResult<IResponse>
{
    private readonly IFusionCache _fusionCache = fusionCache;

    public async Task<TResultOfResponse> Handle(TQuery query, RequestHandlerDelegate<TResultOfResponse> next, CancellationToken cancellationToken)
    {
        var cachedResult = await _fusionCache.GetOrDefaultAsync<TResultOfResponse>(query.CacheKey, token: cancellationToken);

        if (cachedResult is not null)
        {
            return cachedResult;
        }

        var result = await next();

        if (result.IsSuccess)
        {
            await _fusionCache.SetAsync
            (
                query.CacheKey,
                result,
                query.Duration ?? CacheDuration.Default,
                token: cancellationToken
            );
        }

        return result;
    }
}

file static class CacheDuration
{
    internal static TimeSpan Default = TimeSpan.FromMinutes(2);
}