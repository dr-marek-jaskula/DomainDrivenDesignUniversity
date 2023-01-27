﻿namespace Shopway.Domain.Enumerations;

public class RolePermission
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