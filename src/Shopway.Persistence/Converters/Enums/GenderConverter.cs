using Shopway.Domain.Users.Enumerations;

namespace Shopway.Persistence.Converters.Enums;

[GenerateEnumConverter(EnumName = nameof(Gender), EnumNamespace = GenderNamespace)]
public sealed class GenerateGenderConverter
{
    public const string GenderNamespace = "Shopway.Domain.Users.Enumerations";
}
