using Shopway.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class DescriptionConverter : ValueConverter<Description, string>
{
    public DescriptionConverter() : base(description => description.Value, @string => Description.Create(@string).Value) { }
}

public sealed class DescriptionComparer : ValueComparer<Description>
{
    public DescriptionComparer() : base((description1, description2) => description1!.Value == description2!.Value, description => description.GetHashCode()) { }
}