using Shopway.Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class DiscountConverter : ValueConverter<Discount, decimal>
{
    public DiscountConverter() : base(discount => discount.Value, @decimal => Discount.Create(@decimal).Value) { }
}

public sealed class DiscountComparer : ValueComparer<Discount>
{
    public DiscountComparer() : base((discount1, discount2) => discount1!.Value == discount2!.Value, discount => discount.GetHashCode()) { }
}