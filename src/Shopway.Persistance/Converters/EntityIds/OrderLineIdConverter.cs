using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class OrderLineIdConverter : ValueConverter<OrderLineId, Guid>
{
    public OrderLineIdConverter() : base(id => id.Value, guid => OrderLineId.Create(guid)) { }
}