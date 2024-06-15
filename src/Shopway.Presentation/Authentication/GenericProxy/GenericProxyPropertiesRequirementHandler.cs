using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users.Authorization;
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

        var permissionsThatAtLeastOneIsRequired = GetPermissionsThatAtLeastOneIsRequired(resource);

        var userHasPermission = await authorizationService
            .HasPermissionsAsync(userIdResult.Value, LogicalOperation.Or, permissionsThatAtLeastOneIsRequired);

        if (userHasPermission)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail(new AuthorizationFailureReason(this, "Missing required permissions."));
    }

    private static Permission[] GetPermissionsThatAtLeastOneIsRequired(GenericProxyRequirementResource resource)
    {
        return Domain.Users.Enumerations.Permission.List
            .Where(permission => permission.RelatedEntity == resource.EntityType)
            .Where(permission => permission.Type is Domain.Users.Enumerations.PermissionType.Read)
            .Where(permission => permission.HasAllProperties || CanReadRequestedProperties(resource, permission))
            .Select(x => x.RelatedEnum)
            .ToArray();
    }

    private static bool CanReadRequestedProperties(GenericProxyRequirementResource resource, Domain.Users.Enumerations.Permission permission)
    {
        if (permission.Properties is null)
        {
            return false;
        }

        return resource.RequestedProperties
            .Except(permission.Properties)
            .IsEmpty();
    }
}
