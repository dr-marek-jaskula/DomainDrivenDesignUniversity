using Shopway.Domain.EntityIds;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class OrderLineIdConverter : ValueConverter<OrderLineId, string>
{
    public OrderLineIdConverter() : base(id => id.Value.ToString(), ulid => OrderLineId.Create(Ulid.Parse(ulid))) { }
}

public sealed class OrderLineIdComparer : ValueComparer<OrderLineId>
{
    public OrderLineIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}