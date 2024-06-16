using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Presentation.Authentication.GenericProxy;

public sealed class GenericProxyPropertiesRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<GenericProxyPropertiesRequirement, GenericProxyRequirementResource>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GenericProxyPropertiesRequirement requirement, GenericProxyRequirementResource resource)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var authorizationService = scope.ServiceProvider
            .GetRequiredService<IUserAuthorizationService>();

        var userIdResult = authorizationService.GetUserId(context);

        if (userIdResult.IsFailure)
        {
            context.Fail(new AuthorizationFailureReason(this, "Missing user."));
            return;
        }

        var userHasPermission = await authorizationService
            .HasPermissionToReadAsync(userIdResult.Value, resource.Entity, resource.RequestedProperties);

        if (userHasPermission)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail(new AuthorizationFailureReason(this, "Missing required permissions."));
    }
}
