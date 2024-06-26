﻿using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredRolesAttribute(params RoleName[] roles)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public new RoleName[] Roles { get; } = roles;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
