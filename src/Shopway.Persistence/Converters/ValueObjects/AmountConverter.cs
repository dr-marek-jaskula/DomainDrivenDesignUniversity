using Shopway.Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class AmountConverter : ValueConverter<Amount, int>
{
    public AmountConverter() : base(amount => amount.Value, @int => Amount.Create(@int).Value) { }
}

public sealed class AmountComparer : ValueComparer<Amount>
{
    public AmountComparer() : base((amount1, amount2) => amount1!.Value == amount2!.Value, amount => amount.GetHashCode()) { }
}