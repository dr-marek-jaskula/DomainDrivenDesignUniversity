using MediatR;
using System.Reflection;
using Shopway.Domain.Errors;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using System.Linq.Dynamic.Core;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Utilities;
using ZiggyCreatures.Caching.Fusion;
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
        var entityIds = typeof(TRequest)
            .GetProperties()
            .Where(prop => prop.PropertyType.GetInterfaces().Any(interfaceType => interfaceType == typeof(IEntityId)))
            .Select(entityId => entityId.GetValue(request) as IEntityId);

        Error[] errors = entityIds
            .Select(entityId => Validate(entityId!, cancellationToken))
            .Select(task => task.Result)
            .Where(error => error != Error.None)
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return errors.CreateValidationResult<TResponse>();
        }

        return await next();
    }

    private async Task<Error> Validate(IEntityId entityId, CancellationToken cancellationToken)
    {
        var entityType = entityId.GetEntityTypeFromEntityId();

        MethodInfo checkCacheAndDatabasedMethod = GetType()
            .GetFirstGenericMethod(nameof(CheckCacheAndDatabase), entityType, entityId.GetType());

        return await (Task<Error>)checkCacheAndDatabasedMethod.Invoke(this, new object[]
        {
            entityId,
            cancellationToken
        })!;
    }

    public async Task<Error> CheckCacheAndDatabase<TEntity, TEntityId>(TEntityId entityId, CancellationToken cancellationToken)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        var cacheReferenceCheckKey = entityId.ToCacheReferenceCheckKey();

        var isEntityInCache = await _fusionCache.AnyAsync<TEntity, TEntityId>(cacheReferenceCheckKey, cancellationToken);

        if (isEntityInCache)
        {
            return Error.None;
        }

        var isEntityInDatabase = await _context
            .Set<TEntity>()
            .AnyAsync(entityId, cancellationToken);

        if (isEntityInDatabase)
        {
            //We should not store entities in cache using this pipeline, therefore we just store null
            await _fusionCache.SetAsync(cacheReferenceCheckKey, default(TEntity), token: cancellationToken);
            return Error.None;
        }

        return InvalidReference(entityId.Value, typeof(TEntity).Name);
    }
}