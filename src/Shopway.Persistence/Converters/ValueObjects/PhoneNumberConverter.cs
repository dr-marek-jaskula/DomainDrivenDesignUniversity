using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
{
    public PhoneNumberConverter() : base(phoneNumber => phoneNumber.Value, @string => PhoneNumber.Create(@string).Value) { }
}

public sealed class PhoneNumberComparer : ValueComparer<PhoneNumber>
{
    public PhoneNumberComparer() : base((phoneNumber1, phoneNumber2) => phoneNumber1!.Value == phoneNumber2!.Value, phoneNumber => phoneNumber.GetHashCode()) { }
}