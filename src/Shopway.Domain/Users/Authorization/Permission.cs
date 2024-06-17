using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;

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
}

public enum PermissionType
{
    Other = 0,
    Add = 1,
    Update = 2,
    Remove = 3,
    Delete = 4,
    Read = 5,
}

public sealed partial class Permission
{
    private const char _floor = '_';

    public string Name { get; init; }
    public PermissionType Type { get; init; } = PermissionType.Other;
    public string? RelatedAggregateRoot { get; init; }
    public string? RelatedEntity { get; init; }
    public List<string>? Properties { get; init; } = null;

    private Permission(string name)
    {
        Name = name;
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
        if (name.NotContains(_floor))
        {
            Result.Failure<Permission>(Error.InvalidArgument($"Permission must contain '{_floor}'."));
        }

        var entityName = typeof(TEntity).Name;

        if (allowedProperties is not null)
        {
            Result propertiesCheck = IEntityUtilities.ValidateEntityProperties(entityName, allowedProperties);

            if (propertiesCheck!.IsFailure)
            {
                return propertiesCheck.Failure<Permission>();
            }
        }

        return new Permission(name)
        {
            RelatedAggregateRoot = typeof(TAggregateRoot).Name,
            RelatedEntity = entityName,
            Type = permissionType,
            Properties = allowedProperties
        };
    }

    // Empty constructor in this case is required by EF Core
    private Permission()
    {
    }

    public PermissionName GetRelatedEnum()
    {
        return Enum.Parse<PermissionName>(Name);
    }
}
