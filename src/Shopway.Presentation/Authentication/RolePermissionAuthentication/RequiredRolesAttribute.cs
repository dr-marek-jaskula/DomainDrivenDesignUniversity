using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredRolesAttribute(params Role[] roles)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public new Role[] Roles { get; } = roles;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}