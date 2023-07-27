using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Utilities;

public static class IEntityIdUtilities
{
    public static Ulid? Value<TEntityId>(this TEntityId id)
        where TEntityId : IEntityId
    {
        return id is not null
            ? id.Value
            : default(Ulid?);
    }

    public static bool HasValue<TEntityId>(this TEntityId? id)
        where TEntityId : IEntityId
    {
        return id is not null;
    }

    public static IList<Ulid> GetUlids<TEntityId>(this IList<TEntityId> ids)
        where TEntityId : IEntityId
    {
        return ids.Select(x => x.Value).ToList();
    }
}