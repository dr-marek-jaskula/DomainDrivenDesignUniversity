using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.EntityIds;

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
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (Guid.TryParse(userId, out Guid parsedUserId) is false)
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var permissionRepository = scope.ServiceProvider
            .GetRequiredService<IPermissionRepository>();

        HashSet<string> permissions = await permissionRepository
            .GetPermissionsAsync(UserId.Create(parsedUserId));

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
