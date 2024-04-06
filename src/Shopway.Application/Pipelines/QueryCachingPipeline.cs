using MediatR;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.Common.Results;
using System.Text.Json;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Application.Pipelines;


public sealed class QueryCachingPipeline<TRequest, TResultOfResponse>(IFusionCache fusionCache) : IPipelineBehavior<TRequest, TResultOfResponse>
    where TRequest : class, IRequest<TResultOfResponse>, IQuery<IResponse>, ICachedQuery
    where TResultOfResponse : IResult<IResponse>
{
    private readonly IFusionCache _fusionCache = fusionCache;

    public async Task<TResultOfResponse> Handle(TRequest request, RequestHandlerDelegate<TResultOfResponse> next, CancellationToken cancellationToken)
    {
        var cachedResult = await _fusionCache.GetOrDefaultAsync<TResultOfResponse>(request.CacheKey, token: cancellationToken);

        if (cachedResult is not null)
        {
            return cachedResult;
        }

        var result = await next();

        if (result.IsSuccess)
        {
            await _fusionCache.SetAsync
            (
                request.CacheKey,
                result,
                TimeSpan.FromSeconds(30),
                token: cancellationToken
            );
        }

        return result;
    }
}