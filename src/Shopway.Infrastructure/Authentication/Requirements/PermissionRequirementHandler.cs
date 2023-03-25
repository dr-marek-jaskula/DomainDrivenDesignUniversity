using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Domain.EntityIds;
using Shopway.Infrastructure.Abstractions;

namespace Shopway.Infrastructure.Authentication.Requirements;

public sealed class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userId, out Guid parsedUserId) is false)
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        HashSet<string> permissions = await permissionService
            .GetPermissionsAsync(UserId.Create(parsedUserId));

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
