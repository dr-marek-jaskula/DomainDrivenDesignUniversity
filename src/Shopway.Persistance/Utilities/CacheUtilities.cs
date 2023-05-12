using Shopway.Domain.Abstractions;

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
}