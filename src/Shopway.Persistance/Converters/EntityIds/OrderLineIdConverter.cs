using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class OrderLineIdConverter : ValueConverter<OrderLineId, Guid>
{
    public OrderLineIdConverter() : base(id => id.Value, guid => OrderLineId.Create(guid)) { }
}

public sealed class OrderLineIdComparer : ValueComparer<OrderLineId>
{
    public OrderLineIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}