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
    private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

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
                query.Duration ?? _defaultCacheDuration,
                token: cancellationToken
            );
        }

        return result;
    }
}