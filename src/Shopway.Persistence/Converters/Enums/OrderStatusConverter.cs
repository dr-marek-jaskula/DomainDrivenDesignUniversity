using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Persistence.Converters.Enums;

public sealed class OrderStatusConverter : ValueConverter<OrderStatus, string>
{
    public OrderStatusConverter() : base(status => status.ToString(), @string => (OrderStatus)Enum.Parse(typeof(OrderStatus), @string)) { }
}
