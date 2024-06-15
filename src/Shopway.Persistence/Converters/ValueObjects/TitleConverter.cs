using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class TitleConverter : ValueConverter<Title, string>
{
    public TitleConverter() : base(title => title.Value, @string => Title.Create(@string).Value) { }
}

public sealed class TitleComparer : ValueComparer<Title>
{
    public TitleComparer() : base((title1, title2) => title1!.Value == title2!.Value, title => title.GetHashCode()) { }
}
