using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class PaymentIdConverter : ValueConverter<PaymentId, Guid>
{
    public PaymentIdConverter() : base(id => id.Value, guid => PaymentId.Create(guid)) { }
}

public sealed class PaymentIdComparer : ValueComparer<PaymentId>
{
    public PaymentIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}