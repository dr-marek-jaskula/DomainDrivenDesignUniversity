using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Infrastructure.Authentication.PermissionAuthentication.Requirements;

public sealed class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        var userIdResult = permissionService.GetUserId(context);

        if (userIdResult.IsFailure)
        {
            return;
        }

        var userHasPermission = await permissionService
            .HasPermissionAsync(userIdResult.Value, requirement.Permission);

        if (userHasPermission)
        {
            context.Succeed(requirement);
        }
    }
}
