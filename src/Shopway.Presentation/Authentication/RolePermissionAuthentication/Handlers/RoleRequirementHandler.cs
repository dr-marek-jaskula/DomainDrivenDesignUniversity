using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

public sealed class RoleRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<RequiredRolesAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredRolesAttribute requirement)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService>();

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
        }

        context.Fail(new AuthorizationFailureReason(this, "Missing required roles."));
    }
}
