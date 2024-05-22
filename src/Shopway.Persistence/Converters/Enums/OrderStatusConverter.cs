using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Persistence.Converters.Enums;

[GenerateEnumConverter(EnumName = nameof(OrderStatus), EnumNamespace = OrderStatusNamespace)]
public sealed class GenerateOrderStatusConverter
{
    public const string OrderStatusNamespace = "Shopway.Domain.Orders.Enumerations";
}