using Shopway.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class LastNameConverter : ValueConverter<LastName, string>
{
    public LastNameConverter() : base(lastName => lastName.Value, @string => LastName.Create(@string).Value) { }
}

public sealed class LastNameComparer : ValueComparer<LastName>
{
    public LastNameComparer() : base((lastName1, lastName2) => lastName1!.Value == lastName2!.Value, lastName => lastName.GetHashCode()) { }
}