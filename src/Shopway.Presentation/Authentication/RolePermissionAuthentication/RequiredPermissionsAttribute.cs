using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredPermissionsAttribute(params Permission[] permissions)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public Permission[] Permissions { get; } = permissions;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
