using Shopway.Domain.Common.BaseTypes;
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

public sealed partial class Permission : Enumeration<Permission>
{
    private const char _floor = '_';
    public static readonly Permission INVALID_PERMISSION = new(1, nameof(INVALID_PERMISSION));

    public PermissionType Type { get; init; } = PermissionType.Other;
    public string? RelatedAggregateRoot { get; init; }
    public string? RelatedEntity { get; init; }
    public List<string>? Properties { get; init; } = null;

    public Permission(int id, string name)
        : base(id, name)
    {
        if (name.NotContains(_floor))
        {
            throw new ArgumentException($"Permission must contain '{_floor}'.");
        }
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
