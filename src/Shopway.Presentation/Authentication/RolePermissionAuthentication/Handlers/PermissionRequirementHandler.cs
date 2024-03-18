using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

public sealed class PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RequiredPermissionsAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredPermissionsAttribute requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService>();

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