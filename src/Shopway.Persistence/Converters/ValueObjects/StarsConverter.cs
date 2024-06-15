using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class StarsConverter : ValueConverter<Stars, decimal>
{
    public StarsConverter() : base(stars => stars.Value, @decimal => Stars.Create(@decimal).Value) { }
}

public sealed class StarsComparer : ValueComparer<Stars>
{
    public StarsComparer() : base((stars1, stars2) => stars1!.Value == stars2!.Value, stars => stars.GetHashCode()) { }
}
