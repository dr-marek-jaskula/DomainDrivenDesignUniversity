using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

public sealed class RoleRequirementHandler<TPermission, TRole>(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RequiredRolesAttribute<TRole>>
    where TPermission : struct, Enum
    where TRole : struct, Enum
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRolesAttribute<TRole> requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService<TPermission, TRole>>();

        var userIdResult = authorizationService.GetUserId(context);

        if (userIdResult.IsFailure)
        {
            return;
        }

        var userHasRole = await authorizationService
            .HasRolesAsync(userIdResult.Value, requirement.Roles);

        if (userHasRole)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail(new AuthorizationFailureReason(this, "Missing required roles."));
    }
}
