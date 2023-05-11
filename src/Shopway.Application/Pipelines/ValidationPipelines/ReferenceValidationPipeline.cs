using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;
using System.Reflection;
using ZiggyCreatures.Caching.Fusion;
using System.Linq.Dynamic.Core;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Domain.Utilities.ReflectionUtilities;
using static Shopway.Persistence.Utilities.QueryableUtilities;

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

        MethodInfo checkCacheAndDatabasedMethod = typeof(ReferenceValidationPipeline<TRequest, TResponse>)
            .GetFirstGenericMethod(nameof(CheckCacheAndDatabase), entityType, entityId.GetType());

        return await (Task<Error>)checkCacheAndDatabasedMethod.Invoke(this, new object[]
        {
            $"any-{entityType}-{entityId}", //key is just for checking if the reference is correct.
            entityId,
            cancellationToken
        })!;
    }

    public async Task<Error> CheckCacheAndDatabase<TEntity, TEntityId>(string key, TEntityId entityId, CancellationToken cancellationToken)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        var isEntityInCache = (await _fusionCache.TryGetAsync<TEntity>(key, null, cancellationToken)).HasValue;

        if (isEntityInCache)
        {
            return Error.None;
        }

        var isEntityInDatabase = await _context
            .Set<TEntity>()
            .AnyAsync(entityId, cancellationToken);

        if (isEntityInDatabase is false)
        {
            return InvalidReference(entityId.Value, typeof(TEntity).Name);
        }

        //We will not store entities in cache by this pipeline, therefore we store just null in cache
        await _fusionCache.SetAsync(key, default(TEntity), token: cancellationToken);

        return Error.None;
    }
}