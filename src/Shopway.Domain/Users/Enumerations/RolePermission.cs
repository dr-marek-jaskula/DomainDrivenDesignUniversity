namespace Shopway.Domain.Users.Enumerations;

public sealed class RolePermission
{
    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    private RolePermission()
    {
    }

    public int RoleId { get; }

    public int PermissionId { get; }
}
