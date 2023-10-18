using Shopway.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.Enums;

public sealed class GenderConverter : ValueConverter<Gender, string>
{
    public GenderConverter() : base(gender => gender.ToString(), @string => (Gender)Enum.Parse(typeof(Gender), @string)) { }
}
