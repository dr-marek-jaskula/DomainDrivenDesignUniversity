using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Framework;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZiggyCreatures.Caching.Fusion;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.Pipelines.ValidationPipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
{
    private readonly ShopwayDbContext _context;
    private readonly IFusionCache _fusionCache;

    public ReferenceValidationPipeline(ShopwayDbContext context, IFusionCache fusionCache)
    {
        _context = context;
        _fusionCache = fusionCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var referenceProperties = typeof(TRequest)
            .GetProperties()
            .Where(prop => prop.PropertyType.GetInterfaces().Any(x => x == typeof(IEntityId)))
            .ToList();

        Error[] errors = referenceProperties
            .Select(async (reference) => await Validate(reference, request, cancellationToken))
            .Select(task => task.Result)
            .Where(error => error != Error.None)
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return errors.CreateValidationResult<TResponse>();
        }

        _context.ChangeTracker.Clear();

        return await next();
    }

    private async Task<Error> Validate(PropertyInfo reference, TRequest request, CancellationToken cancellationToken)
    {
        //omit optional reference
        if (reference.GetValue(request) is not IEntityId entityId || entityId is null || entityId.Value == Guid.Empty)
        {
            return Error.None;
        }

        var entityType = reference.GetEntityTypeFromEntityId();

        MethodInfo fusionCacheTryGetMethod = GetGenericMethod(typeof(IFusionCache), entityType, "TryGetAsync");
        MethodInfo isInCacheMethod = GetGenericMethod(typeof(ReferenceValidationPipeline<TRequest, TResponse>), entityType, "IsInCache");

        if (await IsEntityInCache(entityId, entityType, fusionCacheTryGetMethod, isInCacheMethod, cancellationToken))
        {
            return Error.None;
        }

        var entity = await _context.FindAsync(entityType, new object[] { entityId }, cancellationToken);

        if (entity is null)
        {
            return InvalidReference(entityId.Value, entityType.Name);
        }

        await _fusionCache.SetAsync($"{entityType}-{entityId}", entity, token: cancellationToken);

        return Error.None;
    }

    public async Task<bool> IsInCache<TType>(MethodInfo methodInfo, string key, CancellationToken cancellationToken)
    {
        var maybe = await (ValueTask<MaybeValue<TType>>)methodInfo.Invoke
        (
            _fusionCache,
            new object[]
            {
                key,
                null,
                cancellationToken
            }
        )!;

        return maybe.HasValue;
    }

    private async Task<bool> IsEntityInCache(IEntityId entityId, Type entityType, MethodInfo fusionCacheTryGetMethod, MethodInfo isInCacheMethod, CancellationToken cancellationToken)
    {
        return await (Task<bool>)isInCacheMethod.Invoke
        (
            this,
            new object[]
            {
                fusionCacheTryGetMethod,
                $"{entityType}-{entityId}",
                cancellationToken
            }
        )!;
    }

    private static MethodInfo GetGenericMethod(Type baseType, Type entityType, string methodName)
    {
        var tryGetAsync = baseType
            .GetMethods()
            .Where(method => method.Name == methodName)
            .First()!;

        return tryGetAsync.MakeGenericMethod(entityType);
    }
}