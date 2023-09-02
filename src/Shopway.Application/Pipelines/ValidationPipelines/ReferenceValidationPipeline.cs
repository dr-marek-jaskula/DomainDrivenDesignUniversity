using MediatR;
using System.Reflection;
using Shopway.Domain.Errors;
using System.Linq.Expressions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using System.Linq.Dynamic.Core;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Utilities;
using ZiggyCreatures.Caching.Fusion;
using System.Collections.ObjectModel;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Domain.Utilities.ReflectionUtilities;
using static Shopway.Persistence.Utilities.QueryableUtilities;

namespace Shopway.Application.Pipelines.ValidationPipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
{
    /// <summary>
    /// This cache stores key-value pairs where: EntityId type is the key and a value is a tuple of corresponding Entity type and generic method that requires these two types.
    /// This cache is provided due to the performance optimizations. We do not want to use reflection calls for each request.
    /// </summary>
    /// <example>Key: typeof(ProductId), Value: (typeof(Product), CheckCacheAndDatabase<Product, ProductId> method)</example>
    private static readonly ReadOnlyDictionary<Type, (Type EntityType, Func<ShopwayDbContext, IFusionCache, IEntityId, CancellationToken, Task<Error>> CheckCacheAndDatabase)> ValidationCache;

    private readonly ShopwayDbContext _context;
    private readonly IFusionCache _fusionCache;

    static ReferenceValidationPipeline()
    {
        Dictionary<Type, (Type EntityType, Func<ShopwayDbContext, IFusionCache, IEntityId, CancellationToken, Task<Error>> CheckCacheAndDatabase)> validationCache = new();
        var entityIdTypes = GetEntityIdTypes();

        foreach (var entityIdType in entityIdTypes)
        {
            Type entityType = GetEntityTypeFromEntityIdType(entityIdType);

            MethodInfo checkCacheAndDatabasedMethod = typeof(ReferenceValidationPipeline<TRequest, TResponse>)
                .GetSingleGenericMethod(nameof(CheckCacheAndDatabase), entityType, entityIdType);

            var compiledFunc = CompileFunc(entityIdType, checkCacheAndDatabasedMethod);

            validationCache.Add(entityIdType, (entityType, compiledFunc!));
        }

        ValidationCache = validationCache.AsReadOnly();
    }

    public ReferenceValidationPipeline(ShopwayDbContext context, IFusionCache fusionCache)
    {
        _context = context;
        _fusionCache = fusionCache;
    }

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

        if (errors.Any())
        {
            return errors.CreateValidationResult<TResponse>();
        }

        return await next();
    }

    private async Task<Error> Validate(IEntityId entityId, CancellationToken cancellationToken)
    {
        return await ValidationCache[entityId.GetType()]
            .CheckCacheAndDatabase(_context, _fusionCache, entityId, cancellationToken);
    }

    public static async Task<Error> CheckCacheAndDatabase<TEntity, TEntityId>(ShopwayDbContext context, IFusionCache cache, TEntityId entityId, CancellationToken cancellationToken)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
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

        return InvalidReference(entityId.Value, typeof(TEntity).Name);
    }

    /// <summary>
    /// This method provides the compiled delegate to the performance will be increased. 
    /// However, if the level of complicity is to hight, we can use store method info in the cache and then compile the method at runtime. 
    /// If so, we can use non static version of CheckCacheAndDatabase method (without context and cache variable explicitly passed as a parameters)
    /// </summary>
    /// <param name="entityIdType"></param>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    private static Func<ShopwayDbContext, IFusionCache, IEntityId, CancellationToken, Task<Error>> CompileFunc(Type entityIdType, MethodInfo methodInfo)
    {
        var param1 = Expression.Parameter(typeof(ShopwayDbContext));
        var param2 = Expression.Parameter(typeof(IFusionCache));
        var param3 = Expression.Parameter(typeof(IEntityId));
        var param4 = Expression.Parameter(typeof(CancellationToken));

        var convertedParameterExpressions = new Expression[]
        {
            param1,
            param2,
            Expression.Convert(param3, entityIdType), //we convert the incorrect type IEntityId (it could be even object) to correct EntityIdType
            param4
        };

        return Expression.Lambda<Func<ShopwayDbContext, IFusionCache, IEntityId, CancellationToken, Task<Error>>>
        (
            Expression.Call(null, methodInfo, convertedParameterExpressions),
            tailCall: false,
            parameters: new[]
            {
                param1,
                param2,
                param3,
                param4
            }
        ).Compile();
    }
}
