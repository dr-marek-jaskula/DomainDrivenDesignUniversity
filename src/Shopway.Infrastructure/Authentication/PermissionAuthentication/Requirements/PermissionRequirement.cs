﻿using Microsoft.AspNetCore.Authorization;

namespace Shopway.Infrastructure.Authentication.PermissionAuthentication.Requirements;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}