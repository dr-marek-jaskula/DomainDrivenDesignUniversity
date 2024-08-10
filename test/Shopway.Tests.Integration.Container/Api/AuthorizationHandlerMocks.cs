using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Api;
public sealed class TestPermissionRequirementHandler : AuthorizationHandler<RequiredPermissionsAttribute<PermissionName>>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredPermissionsAttribute<PermissionName> requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}

public sealed class TestRoleRequirementHandler : AuthorizationHandler<RequiredRolesAttribute<RoleName>>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRolesAttribute<RoleName> requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}

public sealed class TestApiKeyRequirementHandler : AuthorizationHandler<RequiredApiKeyAttribute<ApiKeyName>>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredApiKeyAttribute<ApiKeyName> requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}
