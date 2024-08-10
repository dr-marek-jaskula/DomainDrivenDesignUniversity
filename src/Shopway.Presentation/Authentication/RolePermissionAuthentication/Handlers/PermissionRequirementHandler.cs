using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

public sealed class PermissionRequirementHandler<TPermission, TRole>(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RequiredPermissionsAttribute<TPermission>>
    where TPermission : struct, Enum
    where TRole : struct, Enum
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredPermissionsAttribute<TPermission> requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService<TPermission, TRole>>();

        var userIdResult = authorizationService.GetUserId(context);

        if (userIdResult.IsFailure)
        {
            return;
        }

        var userHasPermission = await authorizationService
            .HasPermissionsAsync(userIdResult.Value, requirement.LogicalOperation, requirement.Permissions);

        if (userHasPermission)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail(new AuthorizationFailureReason(this, "Missing required permissions."));
    }
}
