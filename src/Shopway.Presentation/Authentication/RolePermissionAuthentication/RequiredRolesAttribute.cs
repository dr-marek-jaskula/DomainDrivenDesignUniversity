using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredRolesAttribute<TRole>(params TRole[] roles)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    where TRole : struct, Enum
{
    public new TRole[] Roles { get; } = roles;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
