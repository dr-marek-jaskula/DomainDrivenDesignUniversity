using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class PriceConverter : ValueConverter<Price, decimal>
{
    public PriceConverter() : base(price => price.Value, @decimal => Price.Create(@decimal).Value) { }
}

public sealed class PriceComparer : ValueComparer<Price>
{
    public PriceComparer() : base((price1, price2) => price1!.Value == price2!.Value, price => price.GetHashCode()) { }
}
