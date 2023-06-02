using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class UomCodeConverter : ValueConverter<UomCode, string>
{
    public UomCodeConverter() : base(uomCode => uomCode.Value, @string => UomCode.Create(@string).Value) { }
}

public sealed class UomCodeComparer : ValueComparer<UomCode>
{
    public UomCodeComparer() : base((uomCode1, uomCode2) => uomCode1!.Value == uomCode2!.Value, uomCode => uomCode.GetHashCode()) { }
}