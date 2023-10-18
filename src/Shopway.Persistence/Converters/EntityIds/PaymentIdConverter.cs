using Shopway.Domain.EntityIds;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class PaymentIdConverter : ValueConverter<PaymentId, string>
{
    public PaymentIdConverter() : base(id => id.Value.ToString(), ulid => PaymentId.Create(Ulid.Parse(ulid))) { }
}

public sealed class PaymentIdComparer : ValueComparer<PaymentId>
{
    public PaymentIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}