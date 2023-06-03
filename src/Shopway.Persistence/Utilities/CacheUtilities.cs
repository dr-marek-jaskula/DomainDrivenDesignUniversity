using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Persistence.Utilities;

public static class CacheUtilities
{
    public static string ToCacheReferenceCheckKey(this IEntityId entityId)
    {
        //This key is used just to check if the entity id points to the entity
        return $"any-{entityId.ToCacheKey()}";
    }

    public static string ToCacheKey(this IEntityId entityId)
    {
        var skipAmount = IEntityId.Id.Length;
        var typeName = entityId.GetType().Name[0..^skipAmount];

        return $"{typeName}-{entityId}";
    }

    public static void Update<TEntity, TEntityId>(this IFusionCache fusionCache, TEntity entity)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        fusionCache.Set(entity.Id.ToCacheKey(), entity);
    }

    public static void Remove(this IFusionCache fusionCache, IEntityId entityId)
    {
        fusionCache.Remove(entityId.ToCacheKey());
        fusionCache.Remove(entityId.ToCacheReferenceCheckKey());
    }

    public static void Set<TEntity, TEntityId>(this IFusionCache fusionCache, TEntity entity)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        fusionCache.Set(entity.Id.ToCacheKey(), entity);
        fusionCache.Set(entity.Id.ToCacheReferenceCheckKey(), default(TEntity));
    }

    public static async Task<bool> AnyAsync<TEntity, TEntityId>(this IFusionCache fusionCache, string key, CancellationToken cancellationToken)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId
    {
        return (await fusionCache.TryGetAsync<TEntity>(key, null, cancellationToken)).HasValue;
    }
}