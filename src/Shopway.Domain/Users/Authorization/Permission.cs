using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.BaseTypes.IEntityUtilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.Authorization;

public enum PermissionName
{
    INVALID_PERMISSION = 1,

    Review_Add = 2,
    Review_Update = 3,
    Review_Remove = 4,
    Review_Read = 5,

    Product_Read = 6,
    Product_Read_Customer = 7,
    OrderHeader_Read_Customer = 8,
    User_Read_Customer = 9
}

public sealed partial class Permission
{
    private const char _floor = '_';
    public static Permission INVALID_PERMISSION = new(nameof(INVALID_PERMISSION)) //Test purposes
    {
        Type = PermissionType.Other
    };

    public string Name { get; init; }
    public PermissionType Type { get; init; } = PermissionType.Other;
    public string? RelatedAggregateRoot { get; init; }
    public string? RelatedEntity { get; init; }
    public List<string>? Properties { get; init; } = null;

    private Permission(string name)
    {
        Name = name;
    }

    public static Result<Permission> CreatePermission
    (
        string name, 
        string relatedAggregateRoot,
        string relatedEntity,
        string permissionTypeAsString,
        List<string>? allowedProperties = null
    )
    {
        var parsePermissionTypeResult = Enum.TryParse<PermissionType>(permissionTypeAsString, out var permissionType);

        var errors = Validate(name, relatedAggregateRoot, relatedEntity, parsePermissionTypeResult, permissionTypeAsString, allowedProperties);

        if (errors.NotNullOrEmpty())
        {
            return ValidationResult<Permission>.WithErrors([.. errors]);
        }

        return new Permission(name)
        {
            RelatedAggregateRoot = relatedAggregateRoot,
            RelatedEntity = relatedEntity,
            Type = permissionType,
            Properties = allowedProperties
        };
    }

    public static Result<Permission> CreatePermission<TAggregateRoot, TEntity>
    (
        string name, 
        PermissionType permissionType,
        List<string>? allowedProperties = null
    )
        where TAggregateRoot : class, IAggregateRoot
        where TEntity : class, IEntity
    {
        return CreatePermission(name, typeof(TAggregateRoot).Name, typeof(TEntity).Name, permissionType.ToString(), allowedProperties);
    }

    // Empty constructor in this case is required by EF Core
    private Permission()
    {
    }

    public PermissionName GetRelatedEnum()
    {
        return Enum.Parse<PermissionName>(Name);
    }

    public static IList<Error> Validate
    (
        string name,
        string relatedAggregateRoot,
        string relatedEntity,
        bool parsePermissionTypeResult,
        string permissionTypeAsString,
        List<string>? allowedProperties = null
    )
    {

        return EmptyList<Error>()
            .If(name.NotContains(_floor), Error.InvalidArgument($"Permission must contain '{_floor}'."))
            .If(parsePermissionTypeResult is false, Error.InvalidArgument($"{permissionTypeAsString} is not a valid PermissionType. Valid PermissionTypes: {string.Join(", ", PermissionType.Other.GetEnumNames())}"))
            .If(IsAggregateRoot(relatedAggregateRoot) is false, Error.InvalidArgument($"{relatedAggregateRoot} is not a valid RelatedAggregateRoot"))
            .If(IsEntity(relatedEntity) is false, Error.InvalidArgument($"{relatedEntity} is not a valid Entity"))
            .If(IsEntity(relatedEntity) && ReatedEntityPropertiesAreInvalid(relatedEntity, allowedProperties), Error.InvalidArgument($"{relatedEntity} is not a valid Entity"));
    }

    private static bool ReatedEntityPropertiesAreInvalid(string relatedEntity, List<string>? allowedProperties)
    {
        return allowedProperties is not null && ValidateEntityProperties(relatedEntity, allowedProperties).IsFailure;
    }
}
