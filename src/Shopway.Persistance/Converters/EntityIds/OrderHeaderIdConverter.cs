using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class OrderHeaderIdConverter : ValueConverter<OrderHeaderId, Guid>
{
    public OrderHeaderIdConverter() : base(id => id.Value, guid => OrderHeaderId.Create(guid)) { }
}
