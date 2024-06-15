using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Persistence.Converters.Enums;

[GenerateEnumConverter(EnumName = nameof(PaymentStatus), EnumNamespace = PaymentStatusNamespace)]
public sealed class GeneratePaymentStatusConverter
{
    public const string PaymentStatusNamespace = "Shopway.Domain.Orders.Enumerations";
}
