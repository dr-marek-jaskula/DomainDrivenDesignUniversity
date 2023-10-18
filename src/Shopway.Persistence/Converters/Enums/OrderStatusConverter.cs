using Shopway.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.Enums;

public sealed class OrderStatusConverter : ValueConverter<OrderStatus, string>
{
    public OrderStatusConverter() : base(status => status.ToString(), @string => (OrderStatus)Enum.Parse(typeof(OrderStatus), @string)) { }
}
