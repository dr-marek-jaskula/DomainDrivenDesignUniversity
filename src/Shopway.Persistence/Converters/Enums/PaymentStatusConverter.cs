using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Converters.Enums;

public sealed class PaymentStatusConverter : ValueConverter<PaymentStatus, string>
{
    public PaymentStatusConverter() : base(status => status.ToString(), @string => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), @string)) { }
}
