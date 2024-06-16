using Shopway.Domain.Users.Authorization;

namespace Shopway.Persistence.Converters.Enums;

[GenerateEnumConverter(EnumName = nameof(PermissionType), EnumNamespace = PermissionTypeNamespace)]
public sealed class GeneratePermissionTypeConverter
{
    public const string PermissionTypeNamespace = "Shopway.Domain.Users.Authorization";
}
