using Microsoft.AspNetCore.Authorization;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Api;
public sealed class TestPermissionRequirementHandler : AuthorizationHandler<RequiredPermissionsAttribute>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredPermissionsAttribute requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}

public sealed class TestRoleRequirementHandler : AuthorizationHandler<RequiredRolesAttribute>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRolesAttribute requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}

public sealed class TestApiKeyRequirementHandler : AuthorizationHandler<RequiredApiKeyAttribute>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredApiKeyAttribute requirement)
    {
        foreach (var pendingRequirement in context.PendingRequirements)
        {
            context.Succeed(pendingRequirement);
        }

        return Task.FromResult(requirement);
    }
}