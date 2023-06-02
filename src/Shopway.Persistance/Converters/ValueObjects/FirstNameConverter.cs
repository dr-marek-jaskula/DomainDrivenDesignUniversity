using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class FirstNameConverter : ValueConverter<FirstName, string>
{
    public FirstNameConverter() : base(firstName => firstName.Value, @string => FirstName.Create(@string).Value) { }
}

public sealed class FirstNameComparer : ValueComparer<FirstName>
{
    public FirstNameComparer() : base((firstName1, FirstName2) => firstName1!.Value == FirstName2!.Value, firstName => firstName.GetHashCode()) { }
}