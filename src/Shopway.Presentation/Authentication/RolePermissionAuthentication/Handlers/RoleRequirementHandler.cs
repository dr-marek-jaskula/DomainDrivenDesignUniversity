using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

public sealed class RoleRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RequiredRolesAttribute<RoleName>>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRolesAttribute<RoleName> requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService<PermissionName, RoleName>>();

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
