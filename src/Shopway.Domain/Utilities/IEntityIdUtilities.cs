using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Utilities;

public static class IEntityIdUtilities
{
    public static Guid? Value<TEntityId>(this TEntityId id)
        where TEntityId : IEntityId
    {
        return id is not null
            ? id.Value
            : default(Guid?);
    }

    public static bool HasValue<TEntityId>(this TEntityId? id)
        where TEntityId : IEntityId
    {
        return id is not null;
    }

    public static IList<Guid> GetGuids<TEntityId>(this IList<TEntityId> ids)
        where TEntityId : IEntityId
    {
        return ids.Select(x => x.Value).ToList();
    }
}