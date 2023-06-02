using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shopway.Domain.Abstractions;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class EntityIdComparer : ValueComparer<IEntityId>
{
    public EntityIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}
