using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Results.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Persistence.Framework;
using System.Collections.Frozen;
using System.Linq.Dynamic.Core;
using ZiggyCreatures.Caching.Fusion;
using static Shopway.Application.Cache.ApplicationCache;
using static Shopway.Domain.Common.Utilities.ReflectionUtilities;
using static Shopway.Persistence.Cache.PersistenceCache;
using static Shopway.Persistence.Utilities.QueryableUtilities;

namespace Shopway.Persistence.Pipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse>(ShopwayDbContext context, IFusionCache fusionCache)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, IResult
{
    private readonly ShopwayDbContext _context = context;
    private readonly IFusionCache _fusionCache = fusionCache;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var entityIds = typeof(TRequest)
            .GetProperties()
            .Where(property => property.Implements<IEntityId>())
            .Select(entityId => entityId.GetValue(request) as IEntityId);

        Error[] errors = entityIds
            .Select(entityId => Validate(entityId!, cancellationToken))
            .Select(task => task.Result)
            .Where(error => error != Error.None)
            .Distinct()
            .ToArray();

        if (errors.Length is not 0)
        {
            return (TResponse)ValidationResultCache[typeof(TResponse)](errors);
        }

        return await next();
    }

    private async Task<Error> Validate(IEntityId entityId, CancellationToken cancellationToken)
    {
        return await ValidationCache[entityId.GetType()](_context, _fusionCache, entityId, cancellationToken);
    }

    public static async Task<Error> CheckCacheAndDatabase<TEntity, TEntityId>
    (
        ShopwayDbContext context,
        IFusionCache cache,
        TEntityId entityId,
        CancellationToken cancellationToken
    )
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        var cacheReferenceCheckKey = entityId.ToCacheReferenceCheckKey();

        var isEntityInCache = await cache.AnyAsync<TEntity, TEntityId>(cacheReferenceCheckKey, cancellationToken);

        if (isEntityInCache)
        {
            return Error.None;
        }

        var isEntityInDatabase = await context
            .Set<TEntity>()
            .AnyAsync(entityId, cancellationToken);

        if (isEntityInDatabase)
        {
            //We should not store entities in the cache using this pipeline, therefore we just store null
            await cache.SetAsync(cacheReferenceCheckKey, default(TEntity), token: cancellationToken);
            return Error.None;
        }

        return Error.InvalidReference(entityId.Value, typeof(TEntity).Name);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors, FrozenDictionary<Type, Func<Error[], IValidationResult>> keyValuePairs)
        where TResult : class, IResult
    {
        if (typeof(TResult) == typeof(Result) || typeof(TResult) == typeof(IResult))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        return (TResult)keyValuePairs[typeof(TResult)](errors)!;
    }
}
