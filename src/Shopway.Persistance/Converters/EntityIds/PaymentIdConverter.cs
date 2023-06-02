using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class PaymentIdConverter : ValueConverter<PaymentId, Guid>
{
    public PaymentIdConverter() : base(id => id.Value, guid => PaymentId.Create(guid)) { }
}