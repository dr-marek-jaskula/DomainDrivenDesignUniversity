namespace Shopway.Domain.Users.Authorization;

public sealed class RolePermission
{
    public RolePermission(string roleName, string permissionName)
    {
        RoleName = roleName;
        PermissionName = permissionName;
    }

    private RolePermission()
    {
    }

    public string RoleName { get; init; }

    public string PermissionName { get; init; }
}
